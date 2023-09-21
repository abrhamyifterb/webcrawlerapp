import React, { useRef, useState } from 'react';
import WebsiteList from './WebsiteList';
import WebsiteGraph from './WebsiteGraph';
import { ApolloProvider } from '@apollo/client';
import ApolloClientConfig from '../../apis/graphQL/ApolloClientConfig';

const VisualizationPage = () => {
  const [selectedWebPages, setSelectedWebPages] = useState([]);
  const websiteListRef = useRef(null);

  const handleNewWebPageAdded = () => {
    if (websiteListRef.current) {
      websiteListRef.current.refetchWebsites();
    }
  }
  return (
    <ApolloProvider client={ApolloClientConfig}>
    <div style={{margin: '25px'}}>
      <h1>Visualization</h1>
      
      <WebsiteList ref={websiteListRef} selectedWebPages={selectedWebPages} setSelectedWebPages={setSelectedWebPages} onNewWebPageAdded={handleNewWebPageAdded} />

      <WebsiteGraph webPages={selectedWebPages} setSelectedWebPages={setSelectedWebPages} onNewWebPageAdded={handleNewWebPageAdded} />
    </div>
    </ApolloProvider>
  );
};

export default VisualizationPage;
