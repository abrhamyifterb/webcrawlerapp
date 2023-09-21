import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Layout from './pages/Layout';
import VisualizationPage from './components/graph/VisualizationPage';
import ExecutionList from './components/execution/ExecutionList';
import SiteRecordList from './components/site/SiteRecordList';

function App() {
  return (
    <div>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" exact element={<SiteRecordList />} />
            <Route path="/site-management" exact element={<SiteRecordList />} />
            <Route path="/execution-management" exact element={<ExecutionList />} />
            <Route path="/execution-management/:id" exact element={<ExecutionList />} />
            <Route path="/visualization" exact element={<VisualizationPage />} />
          </Routes>
        </Layout>
      </Router>
    </div>
  );
}

export default App;
