import React, { useState, useEffect } from 'react';
import { useQuery, useMutation } from '@apollo/client';
import { ForceGraph2D } from 'react-force-graph';
import { FETCH_NODES } from './graphql';
import { crawlWebsiteRecord } from '../apiServices';

function WebsiteGraph({ webPages, mode, viewMode }) {
  const { loading, error, data } = useQuery(FETCH_NODES, {
    variables: { webPages },
    pollInterval: mode === 'live' ? 5000 : undefined
  });

  const [graphData, setGraphData] = useState({ nodes: [], links: [] });
  const [currentView, setCurrentView] = useState(viewMode || 'website');

  useEffect(() => {
    if (data && data.nodes) {
      const processedData = currentView === 'website'
        ? processWebsiteView(data.nodes)
        : processDomainView(data.nodes);
      setGraphData(processedData);
    }
  }, [data, currentView]);

  const processWebsiteView = (nodes) => {
    const nodeMap = {};
    const links = [];

    nodes.forEach(node => {
      const nodeId = node.url;
      nodeMap[nodeId] = {
        id: nodeId,
        label: node.title || node.url,
        active: node.owner.active,
        regexp: node.owner.regexp,
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
      const domain = new URL(node.url).hostname;
      if (!domainMap[domain]) {
        domainMap[domain] = {
          id: domain,
          label: domain,
          children: [],
          active: node.owner.active,
          regexp: node.owner.regexp,
          owner: node.owner
        };
      }

      domainMap[domain].children.push(node.url);

      node.links.forEach(link => {
        const targetDomain = new URL(link.url).hostname;
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

  const handleNodeDoubleClick = async (node) => {
    if (node.owner) {
      try {
        await crawlWebsiteRecord(node.owner.identifier); 
        alert('Execution started!');
      } catch (err) {
        alert('Failed to start execution.');
      }
    } else {
      
    }
  };

  const getNodeColor = (node) => {
    if (new RegExp(node.regexp).test(node.url)) {
      return 'red';
    } else if (node.active) {
      return 'green'; 
    } else {
      return 'purple'; 
    }
  };
  

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div>
      <button onClick={() => setCurrentView('website')}>Website View</button>
      <button onClick={() => setCurrentView('domain')}>Domain View</button>

      {graphData && graphData.nodes.length > 0 && (
        <ForceGraph2D
          graphData={graphData}
          nodeLabel="label"
          nodeColor={getNodeColor}
          linkDirectionalArrowLength={3.5}
          linkDirectionalArrowRelPos={1}
          onNodeDoubleClick={handleNodeDoubleClick}
        />
      )}
    </div>
  );
}

export default WebsiteGraph;
