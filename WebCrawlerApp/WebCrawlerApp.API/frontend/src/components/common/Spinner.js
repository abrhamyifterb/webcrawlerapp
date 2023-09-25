import React from 'react';
import './Spinner.scss';

const Spinner = () => {
  return (
    <div className="spinner-overlay">
      <img 
        src={`${process.env.PUBLIC_URL}/spinner.svg`} 
        alt="Loading spinner" 
        className="spinner-image"
      />
    </div>
  );
};

export default Spinner;
