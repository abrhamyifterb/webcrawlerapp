import React, { useState, useEffect } from 'react';
import { SearchCriteriaModel, SortConfigModel } from '../dataModels/dataModels';
import { useFilterRecords } from '../hooks/useFilterRecords';
import { useSortRecords } from '../hooks/useSortRecords';
import { usePaginateRecords } from '../hooks/usePaginateRecords';
import Pagination from './Pagination';
import './ExecutionList.scss';
import TableHeader from './TableHeader';
import TableRowExecution from './TableRowExecution';

import { fetchAllExecutionRecords, fetchExecutionRecordByWebsiteId } from '../apis/executionApiServices';
import { useParams } from 'react-router-dom';
import { crawlWebsiteRecord, fetchWebsiteRecord } from '../apis/apiServices';
import SimpleModal from '../pages/SimpleModal';

const ExecutionList = () => {

  const [executionRecordsList, setExecutionRecordsList] = useState([]);
  const [websiteRecord, setWebsiteRecord] = useState([]);
  const [apiError, setApiError] = useState(null);

  const [modalInfo, setModalInfo] = useState({isVisible: false, title: "", message: "", titleColor: ""});
  const [recordToCrawl, setRecordToCrawl] = useState(null);

  const [searchCriteria, setSearchCriteria] = useState(SearchCriteriaModel);
  const filteredRecords = useFilterRecords(executionRecordsList, searchCriteria);

  const [sortConfig, setSortConfig] = useState(SortConfigModel);
  const sortedRecords = useSortRecords(filteredRecords, sortConfig);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 2; 
  const paginatedData = usePaginateRecords(sortedRecords, itemsPerPage, currentPage);

  const { id } = useParams();

  const handleExecutionSort = (field, order) => {
      setSortConfig({ field, order });
  };

  const handleExecutionSearch = (field, value) => {
    setSearchCriteria(prevState => {
        const updatedCriteria = { ...prevState, [field]: value };
        return updatedCriteria;
    });
  };

  const columns = [
    { label: "Website Label", field: "websiteLabel" },
    { label: "Status", field: "status", enableSort: false, enableSearch: false },
    { label: "StartTime", field: "startTime", enableSearch: false },
    { label: "EndTime", field: "endTime", enableSearch: false },
    { label: "CrawledSitesCount", field: "crawledSitesCount", enableSearch: false }
  ];

  const fetchExecutionRecords = async () => {
    try {
        let data = [];
        let webData = [];
        if(id){
          try{
            webData = await fetchWebsiteRecord(id);
            setWebsiteRecord(webData.data);
          }
          catch (error) {
            console.error("Error fetching web record:", error);
          }
          data = await fetchExecutionRecordByWebsiteId(webData.data.id);
        }
        else {
          data = await fetchAllExecutionRecords();
        }
        console.log(data);
        setExecutionRecordsList(data.data);
        console.log(data.message);
    } catch (error) {
        console.error("Error fetching records:", error);
    }
  };

  const handleCrawlClick = () => {
    setRecordToCrawl(websiteRecord);
    setModalInfo({ isVisible: true, title: "Confirm Crawl", message: "Are you sure you want to trigger crawl for this site?", titleColor: "green" });
  };

  const confirmCrawl = async () => {
    try {
        await crawlWebsiteRecord(websiteRecord.id); 
        setModalInfo({ isVisible: true, title: "Success", message: "Website crawled successfully.", titleColor: "green" });
        fetchExecutionRecords();
        setRecordToCrawl(null);
    } catch (error) {
        setModalInfo({ isVisible: true, title: "Error", message: "Failed to delete the record.", titleColor: "red" });
    }
  };

  const cancelCrawl = () => {
    setRecordToCrawl(null);
    setModalInfo({ isVisible: false, title: "", message: "", titleColor: "" });
  };

  const closeModal = () => {
      setModalInfo({ isVisible: false, title: "", message: "", titleColor: "" });
  };

  useEffect(() => {      
    fetchExecutionRecords();
  }, []);

  return (
    <div className="execution-list">
      <h2>Execution List {(websiteRecord && websiteRecord.id) ? `for ${websiteRecord.url}` : "" }</h2>
      {(websiteRecord && websiteRecord.id) ? (<><a href={`/execution-management`}><button className="add">See All Executions</button></a>  <button className="crawl" onClick={handleCrawlClick}>Trigger Crawl</button></>) : ""}
      <table>
        <thead>
        <tr>
            {columns.map(column => (
              <TableHeader
                key={column.field}
                {...column}
                sortField={sortConfig.field}
                sortOrder={sortConfig.order}
                onSort={handleExecutionSort}
                onSearch={handleExecutionSearch}
              />
            ))}
          </tr>
        </thead>
        <tbody>
          {paginatedData.map((execution) => (
              <TableRowExecution 
                  key={execution.id}
                  execution={execution}
              />
          ))}
        </tbody>
      </table>
      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil(sortedRecords.length / itemsPerPage)}
        onPageChange={setCurrentPage}
      />

      <SimpleModal 
        isVisible={modalInfo.isVisible}
        onClose={closeModal}
        title={modalInfo.title}
        titleColor={modalInfo.titleColor}
        buttons={
          recordToCrawl ?
          <>
            <button className="confirm-btn" onClick={confirmCrawl}>Yes</button>
            <button className="cancel-btn" onClick={cancelCrawl}>No</button>
          </>
          : <button className="modal-close-btn" onClick={closeModal}>Close</button>
        }
      >
        {modalInfo.message}
      </SimpleModal>
    </div>
  );
};

export default ExecutionList;