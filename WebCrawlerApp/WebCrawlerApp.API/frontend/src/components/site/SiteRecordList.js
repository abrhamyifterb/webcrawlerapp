import React, { useState, useEffect, useCallback } from 'react';
import { SearchCriteriaModel, SortConfigModel } from '../../dataModels/dataModels';
import { useFilterRecords } from '../../custom-hooks/useFilterRecords';
import { useSortRecords } from '../../custom-hooks/useSortRecords';
import { usePaginateRecords } from '../../custom-hooks/usePaginateRecords';
import { useUniqueTags } from '../../custom-hooks/useUniqueTags';
import SiteRecordForm from './SiteRecordForm';
import './SiteRecordList.scss';
import TableHeader from '../common/TableHeader';
import TableRowSiteRecord from './TableRowSiteRecord';
import Pagination from '../common/Pagination';
import SimpleModal from '../common/SimpleModal';
import { crawlWebsiteRecord, deleteWebsiteRecord, fetchAllWebsiteRecords } from '../../apis/rest/apiServices';

const SiteRecordList = () => {
    const [showForm, setShowForm] = useState(false);
    const [editingRecord, setEditingRecord] = useState(null);

    const [recordsList, setRecordsList] = useState([]);
    const [apiError, setApiError] = useState(null);

    const [recordToCrawl, setRecordToCrawl] = useState(null);
    const [recordToDelete, setRecordToDelete] = useState(null);
    const [modalInfo, setModalInfo] = useState({isVisible: false, title: "", message: "", titleColor: ""});

    const [searchCriteria, setSearchCriteria] = useState(SearchCriteriaModel);
    const filteredRecords = useFilterRecords(recordsList, searchCriteria);

    const [sortConfig, setSortConfig] = useState(SortConfigModel);
    const sortedRecords = useSortRecords(filteredRecords, sortConfig);

    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 5; 
    const paginatedData = usePaginateRecords(sortedRecords, itemsPerPage, currentPage);

    const uniqueTags = useUniqueTags(recordsList);


    const columns = [
      { label: "Label", field: "label" },
      { label: "Url", field: "url" },
      { label: "RegExp Boundary", field: "boundaryRegExp" },
      { label: "Periodicity", field: "crawlFrequency" },
      { label: "IsActive", field: "isActive", isStatusField: true, enableSort: false },
      { label: "Tags", field: "tags", isTagField: true, uniqueTags, enableSort: false },
      { label: "LastExecutionTime", field: "lastExecutionTime", enableSearch: false}, 
      { label: "Actions", enableSearch: false, enableSort: false },
    ];
    
    const handleAddClick = useCallback(() => {
      setEditingRecord(null);
      setShowForm(true);
    }, []);

    const handleEditClick = useCallback((record) => {
      setEditingRecord(record);
      setShowForm(true);
    }, []);


    const handleCrawlClick = useCallback((record) => {
      setRecordToCrawl(record);
      setModalInfo({ isVisible: true, title: "Confirm Crawl", message: "Are you sure you want to trigger crawl for this site?", titleColor: "green" });
    }, []);
    
    const handleFormCancel = useCallback(() => {
      setShowForm(false);
    }, []);
    
    const handleSort = useCallback((field, order) => {
        setSortConfig({ field, order });
    }, []);
    
    const handleSearch = useCallback((field, value) => {
        setSearchCriteria(prevState => {
            const updatedCriteria = { ...prevState, [field]: value };
            return updatedCriteria;
        });
    }, []);

    const handleFormSuccess = (updatedOrNewRecord) => {
      if (editingRecord) {
          const updatedRecordsList = recordsList.map(record =>
              record.id === editingRecord.id ? updatedOrNewRecord : record
          );
          setRecordsList(updatedRecordsList);
      } else {
          setRecordsList([...recordsList, updatedOrNewRecord]);
      }
      setModalInfo({ isVisible: true, title: "Success", message: `Record ${(editingRecord && editingRecord.id) ? "updated" : "added"} successfully.`, titleColor: "green" });
      setEditingRecord(null);
    };
  
    const handleFormError = (error) => {
      console.error("There was an error with the form:", error);
      setApiError(error);
      setModalInfo({ isVisible: true, title: "Error", message: `There was an error: ${error.message}`, titleColor: "red" });

      console.log(apiError);
    };

    const handleDeleteClick = (record) => {
      setRecordToDelete(record);
      setModalInfo({ isVisible: true, title: "Confirm Delete", message: "Are you sure you want to delete this record?", titleColor: "red" });
  };

    SiteRecordForm.defaultProps = {
      onSubmit: () => {},
    };

    const fetchAllRecords = async () => {
      try {
          const data = await fetchAllWebsiteRecords();
          setRecordsList(data.data);
          console.log(data.message);
      } catch (error) {
          console.error("Error fetching records:", error);
      }
    };

    const confirmCrawl = async () => {
      try {
          setModalInfo({ isVisible: true, title: "Success", message: `Executing/Crawling ...`, titleColor: "green" });
          await crawlWebsiteRecord(recordToCrawl.id); 
          setModalInfo({ isVisible: true, title: "Success", message: "Website crawled successfully.", titleColor: "green" });
          fetchAllRecords();
          setRecordToCrawl(null);
      } catch (error) {
          setModalInfo({ isVisible: true, title: "Error", message: "Failed to delete the record.", titleColor: "red" });
      }
    };

    const confirmDelete = async () => {
        try {
            setModalInfo({ isVisible: true, title: "Success", message: `Deleting ...`, titleColor: "green" });
            await deleteWebsiteRecord(recordToDelete.id); 
            setModalInfo({ isVisible: true, title: "Success", message: "Record deleted successfully.", titleColor: "green" });
            const updatedRecordsList = recordsList.filter(record =>
              record.id !== recordToDelete.id
          );
          setRecordsList(updatedRecordsList);
          setRecordToDelete(null);
        } catch (error) {
            setModalInfo({ isVisible: true, title: "Error", message: "Failed to delete the record.", titleColor: "red" });
        }
    };

    const cancelDelete = () => {
        setRecordToDelete(null);
        setModalInfo({ isVisible: false, title: "", message: "", titleColor: "" });
    };

    const cancelCrawl = () => {
      setRecordToCrawl(null);
      setModalInfo({ isVisible: false, title: "", message: "", titleColor: "" });
    };

    const closeModal = () => {
        setModalInfo({ isVisible: false, title: "", message: "", titleColor: "" });
    };

    useEffect(() => {      
      fetchAllRecords();
    }, []);

    return (
        <div className="site-record-list">
            <h2>Site Record Management</h2>
            <button className="add" onClick={handleAddClick}>Add Site Record</button>
            {showForm && (
                <SiteRecordForm
                    key={editingRecord ? editingRecord.id : 'new'}
                    onCancel={handleFormCancel}
                    onSuccess={handleFormSuccess}
                    onError={handleFormError}
                    initialValues={editingRecord}
                />
            )}
            <table>
                <thead>
                  <tr>
                    {columns.map(column => (
                      <TableHeader
                        key={column.label}
                        {...column}
                        sortField={sortConfig.field}
                        sortOrder={sortConfig.order}
                        onSort={handleSort}
                        onSearch={handleSearch}
                      />
                    ))}
                  </tr>
                </thead>
                <tbody>
                  {paginatedData.length > 0 ? (
                      paginatedData.map((site) => (
                          <TableRowSiteRecord 
                              key={site.id}
                              site={site} 
                              handleCrawlClick={handleCrawlClick}
                              handleEditClick={handleEditClick}
                              handleDeleteClick={handleDeleteClick}
                          />
                      ))
                  ) : (
                      <tr>
                          <td colSpan={columns.length}>No values</td>
                      </tr>
                  )}
                </tbody>
            </table>
            {paginatedData.length > 0 && (
            <Pagination
                currentPage={currentPage}
                totalPages={Math.ceil(sortedRecords.length / itemsPerPage)}
                onPageChange={setCurrentPage}
            />
            )}
            <SimpleModal 
                isVisible={modalInfo.isVisible}
                onClose={closeModal}
                title={modalInfo.title}
                titleColor={modalInfo.titleColor}
                buttons={
                  recordToDelete ? 
                  <>
                    <button className="confirm-btn" onClick={confirmDelete}>Yes</button>
                    <button className="cancel-btn" onClick={cancelDelete}>No</button>
                  </>
                  : recordToCrawl ?
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

export default SiteRecordList;
