import { useState, useEffect } from 'react';

export const usePaginateRecords = (records, itemsPerPage, currentPage) => {
    const [paginatedRecords, setPaginatedRecords] = useState([]);

    useEffect(() => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        setPaginatedRecords(records.slice(startIndex, endIndex));
    }, [records, currentPage, itemsPerPage]);

    return paginatedRecords;
};
