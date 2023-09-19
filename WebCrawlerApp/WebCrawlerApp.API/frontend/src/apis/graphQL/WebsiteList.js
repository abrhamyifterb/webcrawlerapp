import React, { useState } from 'react';
import { useQuery } from '@apollo/client';
import { FETCH_WEBSITES } from './graphql';

function WebsiteList({ setSelectedWebPages }) {
  const { loading, error, data } = useQuery(FETCH_WEBSITES);
  
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10; 

  const lastItem = currentPage * itemsPerPage;
  const firstItem = lastItem - itemsPerPage;
  const currentItems = data?.websites.slice(firstItem, lastItem) || [];

  const pageCount = Math.ceil(data?.websites.length / itemsPerPage);

  const handleSelection = (identifier) => {
    setSelectedWebPages(prev => [...prev, identifier]);
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div>
      <ul>
        {currentItems.map(website => (
          <li key={website.identifier}>
            <input 
              type="checkbox" 
              onChange={() => handleSelection(website.identifier)} 
            />
            {website.label} - {website.url}
          </li>
        ))}
      </ul>
      
      <div>
        {Array.from({ length: pageCount }).map((_, index) => (
          <button key={index} onClick={() => setCurrentPage(index + 1)}>
            {index + 1}
          </button>
        ))}
      </div>
    </div>
  );
}

export default WebsiteList;
