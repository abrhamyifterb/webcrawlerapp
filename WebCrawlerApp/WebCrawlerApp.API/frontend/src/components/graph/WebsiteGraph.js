import React, { useState, useEffect, useCallback, useRef } from 'react';
import { useQuery } from '@apollo/client';
import { ForceGraph2D } from 'react-force-graph';
import { FETCH_NODES } from '../../apis/graphQL/graphql';
import { crawlWebsiteRecord } from '../../apis/rest/apiServices';
import SimpleModal from '../common/SimpleModal';
import { fetchExecutionRecordsByWebsiteIds } from '../../apis/rest/executionApiServices';
import { SiteRecordModel } from '../../dataModels/dataModels';
import SiteRecordForm from '../site/SiteRecordForm';
import './WebsiteGraph.scss';
import Spinner from '../common/Spinner';

const WebsiteGraph = ({ webPages, setSelectedWebPages, onNewWebPageAdded }) => {

  const [mode, setMode] = useState('static');
  const [viewMode, setViewMode] = useState('website');

  const { loading, error, data, refetch } = useQuery(FETCH_NODES, {
    variables: { webPages },
    pollInterval: mode === 'live' ? 5000 : undefined
  });

  const [graphData, setGraphData] = useState({ nodes: [], links: [] });

  const [showForm, setShowForm] = useState(false);
  const [editingRecord, setEditingRecord] = useState(null);
  const [lastEndTime, setLastEndTime] = useState(null);

  const [modalInfo, setModalInfo] = useState({isVisible: false, title: "", message: "", titleColor: "", recordType: null, record: null});
  const [showSpinner, setShowSpinner] = useState(false);

  const clickTimeout = useRef(null);

  const closeModal = () => {
    setModalInfo({isVisible: false, title: "", message: "", titleColor: "", recordType: null, record: null});
  };


  const handleExecute = useCallback(async (node) => {
    try {
      setShowSpinner(true);
      await crawlWebsiteRecord(node && node.owner.identifier ? node.owner.identifier : node.id);
      setShowSpinner(false);
      setModalInfo({isVisible: true, title: "Success", message: "Execution Ended!", titleColor: "green", recordType: null, record: null});
    } catch (err) {
      setShowSpinner(false);
      setModalInfo({isVisible: true, title: "Error", message: "Failed to start execution.", titleColor: "red", recordType: null, record: null});
    }
  }, []);

  const handleCreateAndExecute = useCallback((node) => {
    closeModal();
    const toBeCreated = SiteRecordModel;
    toBeCreated.url = node.url;
    setEditingRecord(toBeCreated);
    setShowForm(true);
  }, []);

  const handleFormCancel = () => {
    setShowForm(false);
  };

  const handleFormSuccess = async (newRecord) => {
    setModalInfo({ isVisible: true, title: "Success", message: `Record added successfully.`, titleColor: "green" });
    setShowForm(false);
    setEditingRecord(null);
    await handleExecute(newRecord);
    setSelectedWebPages(prev => [...prev, newRecord.id]);
    console.log("reached here");
    setMode('live');
    onNewWebPageAdded();
  };

  SiteRecordForm.defaultProps = {
    onSubmit: () => {},
  };

  const handleFormError = (error) => {
    console.error("There was an error with the form:", error);
    setModalInfo({ isVisible: true, title: "Error", message: `There was an error: ${error.message}`, titleColor: "red" });
  };

  useEffect(() => {
    const fetchLatestExecution = async () => {
      const latestExecutionResponse = await fetchExecutionRecordsByWebsiteIds(webPages);
      const latestExecution = latestExecutionResponse && latestExecutionResponse.data?latestExecutionResponse.data : null;
      if (latestExecution && latestExecution.EndTime !== lastEndTime) {
        setLastEndTime(latestExecution.EndTime);
        refetch();
      }
    }
    fetchLatestExecution();

    if (data && data.nodes) {
      const processedData = viewMode === 'website'
        ? processWebsiteView(data.nodes)
        : processDomainView(data.nodes);

        processedData.links = processedData.links.filter(link => 
          processedData.nodes.some(node => node.id === link.source) && 
          processedData.nodes.some(node => node.id === link.target)
      );

      setGraphData(processedData);
    }
    
  }, [data, viewMode, webPages, refetch, lastEndTime]);

  const processWebsiteView = (nodes) => {
    const nodeMap = {};
    const links = [];

    nodes.forEach(node => {
      const nodeId = node.url;
      nodeMap[nodeId] = {
        id: nodeId,
        url: node.url,
        label: (node.title && (node.title !== "No title found")) ? node.title : node.url,
        active: node.owner.active,
        crawlTime: node.crawlTime,
        regexp: node.owner.regexp,
        links: node.links,
        owner: node.owner
      };

      node.links.forEach(link => {
        links.push({
          source: nodeId,
          target: link.url
        });
      });
    });

    return {
      nodes: Object.values(nodeMap),
      links
    };
  };

  const processDomainView = (nodes) => {
    const domainMap = {};
    const links = [];

    nodes.forEach(node => {
      const domainURL = new URL(node.url);
      const domain = domainURL.protocol ? domainURL.protocol + "//" + domainURL.hostname : domainURL.hostname;
      if (!domainMap[domain]) {
        domainMap[domain] = {
          id: domain,
          url: domain,
          label: domain,
          children: [],
          active: node.owner.active,
          regexp: node.owner.regexp,
          owner: node.owner
        };
      }

      domainMap[domain].children.push(node.url);

      node.links.forEach(link => {
        const targetDomainURL = new URL(link.url);
        const targetDomain = targetDomainURL.protocol ? targetDomainURL.protocol + "//" + targetDomainURL.hostname : targetDomainURL.hostname;
        if (domain !== targetDomain) {
          links.push({
            source: domain,
            target: targetDomain
          });
        }
      });
    });

    return {
      nodes: Object.values(domainMap),
      links
    };
  };
  
  const NodeDetail = ({ node, onExecute, onCreateAndExecute }) => {
    if (!node) return null;

    const matchesRegexp = new RegExp(node.regexp).test(node.url);
  
    return (
      <>
        <p>URL: {node.url}</p>
        {node.crawlTime && <p>Crawl time: {node.crawlTime} ms</p>}
        {matchesRegexp ? (
          <>
            <p>Crawled By:</p>
            <ul>
              {node.links.map(link => (
                <li key={link.url}>
                  {link.title || link.url}
                </li>
              ))}
            </ul>
            <button className="crawl" onClick={() => onExecute(node)}>Start Execution</button>
          </>
        ) : (
          <button className="add" onClick={() => onCreateAndExecute(node)}>Create & Execute New Record</button>
        )}
      </>
    );
  }
  
  const handleNodeClick = useCallback((node) => {
    if (clickTimeout.current !== null) {
      clearTimeout(clickTimeout.current);
      clickTimeout.current = null;
      setModalInfo({
        isVisible: true,
        title: "Node Details",
        recordType: 'nodeDetail',
        record: node
      });
    } else {
      clickTimeout.current = setTimeout(() => {
        clickTimeout.current = null;
      }, 300);
    }
  }, []);
  

  const getNodeColor = (node) => {
    if (new RegExp(node.regexp).test(node.url)) {
      return 'green'; 
    } else if (node.active) {
      return 'red'; 
    }
  };  

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div className='website-graph-visual'>
      {showSpinner && <Spinner />}
      {showForm && (
          <SiteRecordForm
              key='new'
              onCancel={handleFormCancel}
              onSuccess={handleFormSuccess}
              onError={handleFormError}
              initialValues={editingRecord}
          />
      )}
      
      {graphData && graphData.nodes.length > 0 && (
        <>
        <button className="edit" onClick={() => setMode(prev => prev === 'live' ? 'static' : 'live')}>Current Mode - {mode}</button>
        <button className="add" onClick={() => setViewMode(prev => prev === 'domain' ? 'website' : 'domain')}>Current View Mode - {viewMode}</button>
        </>
       )}
      {graphData && graphData.nodes.length > 0 && (
      <ForceGraph2D
        graphData={graphData}
        nodeId="id"
        nodeLabel="label"
        linkDirectionalArrowRelPos={1}
        linkDirectionalArrowLength={7}
        onNodeDragEnd={(node) => {
          node.fx = node.x;
          node.fy = node.y;
          node.fz = node.z;
        }}
        nodeCanvasObjectMode={() => "after"}
        nodeCanvasObject={(node, ctx, globalScale) => {
          const nodeLabel = node.label;
          const fontSize = 12 / globalScale;
          ctx.font = `${fontSize}px Sans-Serif`;
          ctx.textAlign = "center";
          ctx.textBaseline = "middle";
          ctx.fillStyle = "black";
          ctx.fillText(nodeLabel, node.x, node.y);
        }}
        nodeColor={getNodeColor}
        onNodeClick={handleNodeClick}
        />
      )}

      <SimpleModal 
        isVisible={modalInfo.isVisible}
        onClose={closeModal}
        title={modalInfo.title}
        titleColor={modalInfo.titleColor}
        buttons={
         <button onClick={closeModal}>Close</button>
        }
      >
        {modalInfo.message}
        {
          modalInfo.recordType === 'nodeDetail' && 
          <NodeDetail 
            node={modalInfo.record}
            onExecute={handleExecute}
            onCreateAndExecute={handleCreateAndExecute}
          />
        }
      </SimpleModal>
    </div>
  );
}

export default WebsiteGraph;
