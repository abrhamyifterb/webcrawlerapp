import { useEffect, useState } from 'react';

export const useFilterRecords = (records, criteria) => {
    const [filteredRecords, setFilteredRecords] = useState([]);
  
    useEffect(() => {
        const actualRecords = records || [];
        
        const filtered = actualRecords.filter((record) => {
        let isValid = true;
  
        for (let key in criteria) {
          const searchValue = criteria[key];
          if (searchValue === null || searchValue === '' || typeof searchValue === 'undefined') continue;
  
          if (key === 'searchTags' && Array.isArray(searchValue) && searchValue.length > 0) {
            isValid = isValid && searchValue.every(tag => record.tags.includes(tag));
          } else if (key === 'searchActiveStatus') {
            isValid = isValid && (record.isActive === (searchValue === 'true'));
          } else if (key === 'searchPeriodicity' || key === 'CrawledSitesCount') {
            isValid = isValid && (record[key] === parseFloat(searchValue));
          } else {
            const recordValue = String(record[key]).toLowerCase();
            isValid = isValid && recordValue.includes(String(searchValue).toLowerCase());
          }
        }
  
        return isValid;
      });
  
      setFilteredRecords(filtered);
    }, [records, criteria]);
  
    return filteredRecords;
  };
  