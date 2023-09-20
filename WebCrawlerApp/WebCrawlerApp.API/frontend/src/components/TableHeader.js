import React from 'react';
import Select from 'react-select';
import './TableHeader.scss';

const TableHeader = ({ 
  label, 
  field, 
  sortField, 
  sortOrder, 
  onSort, 
  onSearch, 
  enableSearch = true, 
  enableSort = true, 
  isTagField = false,
  uniqueTags,
  isStatusField = false
}) => {
  return (
    <th>
      {label}
      {enableSearch && !isTagField && !isStatusField && (
        <input
          type="text"
          placeholder={`Search by ${label.toLowerCase()}`}
          onChange={(e) => onSearch(field, e.target.value)}
        />
      )}
      {isTagField && (
        <Select
          isMulti
          options={uniqueTags.map((tag) => ({ value: tag, label: tag }))}
          onChange={(selectedOptions) => {
            onSearch(field, selectedOptions ? selectedOptions.map((option) => option.value) : []);
          }}
        />
      )}
      {isStatusField && (
        <select onChange={(e) => onSearch(field, e.target.value)}>
          <option value="">All</option>
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      )}
      {enableSort && (
        <>
          <span onClick={() => onSort(field, 'asc')}>
            {sortField === field && sortOrder === 'asc' ? <strong>⏫</strong> : '▲'}
          </span>
          <span onClick={() => onSort(field, 'desc')}>
            {sortField === field && sortOrder === 'desc' ? <strong>⏬</strong> : '▼'}
          </span>
        </>
      )}
    </th>
  );
};

export default TableHeader;
