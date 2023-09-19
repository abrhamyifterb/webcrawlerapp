import React from 'react';
import Button from './Button';
import './Pagination.scss';

const Pagination = ({ currentPage, totalPages, onPageChange }) => {
  return (
    <div className="pagination">
      <Button
        disabled={currentPage === 1}
        onClick={() => onPageChange(currentPage - 1)}
        className={currentPage === 1 ? 'disabled' : ''}
      >
        &laquo;
      </Button>
      {Array.from({ length: totalPages }, (_, index) => (
        <Button
          key={index}
          onClick={() => onPageChange(index + 1)}
          className={currentPage === index + 1 ? 'disabled' : ''}
        >
          {index + 1}
        </Button>
      ))}
      <Button
        disabled={currentPage === totalPages}
        onClick={() => onPageChange(currentPage + 1)}
        className={currentPage === totalPages ? 'disabled' : ''}
      >
        &raquo;
      </Button>
    </div>
  );
};

export default Pagination;