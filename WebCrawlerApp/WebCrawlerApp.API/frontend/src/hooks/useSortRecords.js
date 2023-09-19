import { useState, useEffect } from 'react';

export const useSortRecords = (records, sortConfig) => {
    const [sortedRecords, setSortedRecords] = useState(records);

    useEffect(() => {
        const sorted = [...records].sort((a, b) => {
            if (sortConfig.field === 'crawlFrequency' || sortConfig.field === 'crawledSitesCount') {
                return sortConfig.order === 'asc' ? a[sortConfig.field] - b[sortConfig.field] : b[sortConfig.field] - a[sortConfig.field];
            } else {
                return sortConfig.order === 'asc' ? a[sortConfig.field].localeCompare(b[sortConfig.field]) : b[sortConfig.field].localeCompare(a[sortConfig.field]);
            }
        });
        setSortedRecords(sorted);
    }, [records, sortConfig]);

    return sortedRecords;
};