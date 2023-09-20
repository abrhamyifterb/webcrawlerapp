import React from 'react';
import { NavLink } from 'react-router-dom';
import './Sidebar.scss';

const Sidebar = () => {
  return (
    <div className="sidebar">
      <ul>
        <li><NavLink className={({ isActive }) => (isActive ? 'active-link' : '')} to="/site-management">Site Management</NavLink></li>
        <li><NavLink className={({ isActive }) => (isActive ? 'active-link' : '')}  to="/execution-management">Execution Management</NavLink></li>
        <li><NavLink className={({ isActive }) => (isActive ? 'active-link' : '')} to="/visualization">Visualization</NavLink></li>
      </ul>
    </div>
  );
};

export default Sidebar;