import React from 'react';

const ExecutionDetails = ({ execution }) => {
  return (
    <div>
      <h2>{execution.label}</h2>
      <p>Status: {execution.status}</p>
      <p>Start Time: {execution.startTime}</p>
      <p>End Time: {execution.endTime}</p>
      <p>Sites Crawled: {execution.sitesCrawled}</p>
    </div>
  );
};

export default ExecutionDetails;
