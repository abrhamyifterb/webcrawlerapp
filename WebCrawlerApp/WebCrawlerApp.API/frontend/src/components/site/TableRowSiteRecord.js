import React from 'react';
import './TableRowSiteRecord.scss';

const TableRowSiteRecord = ({ site, handleCrawlClick, handleEditClick, handleDeleteClick }) => {
  return (
    <tr key={site.id}>
      <td>{site.label}</td>
      <td>{site.url}</td>
      <td>{site.boundaryRegExp}</td>
      <td>{site.crawlFrequency}</td>
      <td>{site.isActive ? "Active" : "Inactive"}</td>
      <td>{site.tags.join(', ')}</td>
      <td>{site.lastExecutionTime}</td>
      <td>
        <a href={`/execution-management/${site.id}`}><button className="crawl">Executions</button></a>
        <button className="crawl" onClick={() => handleCrawlClick(site)}>Crawl</button>
        <button className="edit" onClick={() => handleEditClick(site)}>Edit</button>
        <button className="delete" onClick={() => handleDeleteClick(site)}>Delete</button>
      </td>
    </tr>
  );
};

export default TableRowSiteRecord;
