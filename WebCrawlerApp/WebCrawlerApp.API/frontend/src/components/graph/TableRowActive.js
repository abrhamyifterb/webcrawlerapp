import React from 'react';

const TableRowActive = ({ website, selectedWebPages, handleSelection }) => {
  return (
    <tr key={website.identifier}>
      <td>
        <input 
              type="checkbox" 
              checked={selectedWebPages.includes(website.identifier)}
              onChange={() => handleSelection(website.identifier)} 
            />
        </td>
      <td>{website.url}</td>
    </tr>
  );
};

export default TableRowActive;
 