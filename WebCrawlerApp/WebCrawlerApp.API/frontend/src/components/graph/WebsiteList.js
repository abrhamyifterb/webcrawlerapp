import React, { forwardRef, useCallback, useEffect, useImperativeHandle, useState } from 'react';
import { useQuery } from '@apollo/client';
import { FETCH_WEBSITES } from '../../apis/graphQL/graphql';
import Pagination from '../common/Pagination';
import TableHeader from '../common/TableHeader';
import TableRowActive from './TableRowActive';
import './WebsiteList.scss';

const itemsPerPage = 5; 

const WebsiteList = forwardRef((props, ref) => {
  const { loading, error, data, refetch } = useQuery(FETCH_WEBSITES, { fetchPolicy: 'network-only' });
  const [currentItems, setCurrentItems] = useState([]);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [refetchCount, setRefetchCount] = useState(0);

  const updateRefetchedWebsites = useCallback(() => {
    if (data && data.websites) {
      const lastItem = currentPage * itemsPerPage;
      const firstItem = lastItem - itemsPerPage;
      setCurrentItems(data.websites.slice(firstItem, lastItem));
      setPageCount(Math.ceil(data?.websites.length / itemsPerPage));
    }
  }, [data, currentPage, itemsPerPage]);

  const handleRefetch = async () => {
    try {
      console.log("Refetching websites...");
      await refetch();
      updateRefetchedWebsites();
      setRefetchCount(prev => prev + 1);
      console.log("Refetched websites");
    } catch (err) {
      console.error("Error refetching:", err);
    }
  };
  
  useEffect(() => {
    updateRefetchedWebsites();
  }, [data, currentPage, updateRefetchedWebsites, refetchCount]);

  useImperativeHandle(ref, () => ({
    refetchWebsites: handleRefetch
  }));

  const handleSelection = (identifier) => {
    props.setSelectedWebPages(prev => {
      if (prev.includes(identifier)) {
        return prev.filter(id => id !== identifier); 
      } else {
        return [...prev, identifier];
      }
    });
  };
  
  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  const columns = [
    { label: "Active Selection", field: "identifier", enableSort: false, enableSearch: false },
    { label: "URL", field: "label", enableSort: false, enableSearch: false }
  ];

  return (
    <div className="website-list-visual">
      <table>
        <thead>
          <tr>
            {columns.map(column => (
              <TableHeader
                key={column.field}
                {...column}
              />
            ))}
          </tr>
        </thead>
        <tbody>
          {currentItems.length > 0 ? (
            currentItems.map((website) => (
                <TableRowActive 
                    key={website.identifier}
                    website={website}
                    selectedWebPages={props.selectedWebPages}
                    handleSelection={handleSelection}
                />
            ))
            ) : (
                <tr>
                    <td colSpan={columns.length}>No values</td>
                </tr>
            )}
        </tbody>
      </table>
      <Pagination
        currentPage={currentPage}
        totalPages={pageCount}
        onPageChange={setCurrentPage}
      />
    </div>
  );
})

export default WebsiteList;