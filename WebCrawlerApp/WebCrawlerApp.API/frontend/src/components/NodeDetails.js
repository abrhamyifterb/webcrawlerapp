import React from 'react';

function NodeDetails({ node, onClose }) {
  return (
    <div className="node-details">
      <h2>Node Details</h2>
      <p>URL: {node.id}</p>
      {node.crawled && (
        <>
          <p>Crawl time: {node.crawlTime}</p>
          <p>Records crawled: {node.records}</p>
        </>
      )}
      <button onClick={onClose}>Close</button>
    </div>
  );
}

export default NodeDetails;