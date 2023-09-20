import React, { useState, useEffect } from 'react';
import SiteRecordList from '../components/SiteRecordList';
import { siteRecords } from '../apis/mockData';

const SiteManagementPage = () => {

    const [record, setRecords] = useState([]);

    useEffect(() => {
      const loadRecords = async () => {
        const data = siteRecords;
        setRecords(data);
      };
      loadRecords();
    }, []);

  return (
    <SiteRecordList />
  );
};

export default SiteManagementPage;
