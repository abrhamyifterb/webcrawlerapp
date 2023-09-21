import React from 'react';
import './SimpleModal.scss';

const SimpleModal = ({ isVisible, onClose, title, children, titleColor, buttons }) => {
    if (!isVisible) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <h2 style={{ color: titleColor || 'black' }}>{title}</h2>
                <>{children}</>
                {buttons ? buttons : <button className="modal-close-btn" onClick={onClose}>Close</button>}
            </div>
        </div>
    );
};

export default SimpleModal;