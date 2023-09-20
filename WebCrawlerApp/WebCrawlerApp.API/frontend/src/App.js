import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Layout from './pages/Layout';
import SiteManagementPage from './pages/SiteManagementPage';
import ExecutionManagementPage from './pages/ExecutionManagementPage';
import VisualizationPage from './pages/VisualizationPage';
import ExecutionList from './components/ExecutionList';
import { BrowserRouter } from 'react-router-dom';

function App() {
  return (
    <div>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" exact element={<SiteManagementPage />} />
            <Route path="/site-management" exact element={<SiteManagementPage />} />
            <Route path="/execution-management" exact element={<ExecutionManagementPage />} />
            <Route path="/execution-management/:id" exact element={<ExecutionList />} />
            <Route path="/visualization" exact element={<VisualizationPage />} />
          </Routes>
        </Layout>
      </Router>
    </div>
  );
}

export default App;
