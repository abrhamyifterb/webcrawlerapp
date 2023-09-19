import React, { useState } from 'react';
import VisualizationGraph from '../components/VisualizationGraph';
import WebsiteList from '../apis/graphQL/WebsiteList';
import WebsiteGraph from '../apis/graphQL/WebsiteGraph';
import { ApolloProvider } from '@apollo/client';
import ApolloClientConfig from '../apis/graphQL/ApolloClientConfig';

const VisualizationPage = () => {
  const nodes = [
    { id: 1, title: 'Example Node 1', url: 'https://example.com/1' },
    { id: 2, title: 'Example Node 2', url: 'https://example.com/2' },
  ];
  const links = [
    { source: 1, target: 2 },
  ];
  const mockNodes = [
    { id: 'A', crawled: true },
    { id: 'B', crawled: true },
    { id: 'C', crawled: false },
    { id: 'D', crawled: true },
    { id: 'E', crawled: true },
  ];
  
  const mockLinks = [
    { source: 'A', target: 'B' },
    { source: 'A', target: 'C' },
    { source: 'B', target: 'D' },
    { source: 'C', target: 'E' },
    { source: 'D', target: 'E' },
  ];
  const [selectedWebPages, setSelectedWebPages] = useState([]);
  const [mode, setMode] = useState('static');
  const [viewMode, setViewMode] = useState('website');

  return (
    <ApolloProvider client={ApolloClientConfig}>
    <div>
      <h1>Website Crawler</h1>
      <h1>Visualization</h1>
      
      <button onClick={() => setMode(prev => prev === 'live' ? 'static' : 'live')}>Toggle Mode</button>
      
      <button onClick={() => setViewMode(prev => prev === 'website' ? 'domain' : 'website')}>Toggle View</button>

      <WebsiteList setSelectedWebPages={setSelectedWebPages} />

      <WebsiteGraph webPages={selectedWebPages} mode={mode} viewMode={viewMode} />
    </div>
    </ApolloProvider>
  );
};

export default VisualizationPage;
