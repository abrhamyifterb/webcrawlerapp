import React from 'react';

const TableRowExecution = ({ execution }) => {
  return (
    <tr key={execution.id}>
      <td>{execution.websiteLabel}</td>
      <td>{execution.status}</td>
      <td>{execution.startTime}</td>
      <td>{execution.endTime}</td>
      <td>{execution.crawledSitesCount}</td>
    </tr>
  );
};

export default TableRowExecution;
