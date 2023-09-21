import React from 'react';
import Button from './Button';
import './Pagination.scss';

const Pagination = ({ currentPage, totalPages, onPageChange }) => {
  const numNeighbors = 2;

  const generatePages = () => {
    const pages = [];
    for (let i = 1; i <= totalPages; i++) {
      if (i === 1 || i === totalPages || i >= currentPage - numNeighbors && i <= currentPage + numNeighbors) {
        pages.push(i);
      }
    }
    return pages;
  };

  const pagesToShow = generatePages();

  return (
    <div className="pagination">
      <Button
        disabled={currentPage === 1}
        onClick={() => onPageChange(currentPage - 1)}
        className={currentPage === 1 ? 'disabled' : ''}
      >
        &laquo;
      </Button>

      {pagesToShow.map((page, index) => {
        if (index > 0 && pagesToShow[index - 1] !== page - 1) {
          return (
            <React.Fragment key={page}>
              <span>...</span>
              <Button
                onClick={() => onPageChange(page)}
                className={currentPage === page ? 'disabled' : ''}
              >
                {page}
              </Button>
            </React.Fragment>
          );
        }
        return (
          <Button
            key={page}
            onClick={() => onPageChange(page)}
            className={currentPage === page ? 'disabled' : ''}
          >
            {page}
          </Button>
        );
      })}

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