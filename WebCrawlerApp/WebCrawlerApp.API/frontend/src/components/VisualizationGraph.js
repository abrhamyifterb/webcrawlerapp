import React, { useState, useEffect, useRef } from 'react';
import * as d3 from 'd3';
import NodeDetails from './NodeDetails';

function VisualizationGraph({ nodes, links }) {
  const svgRef = useRef();
  const [viewMode, setViewMode] = useState('website');
  const [liveMode, setLiveMode] = useState(false);
  const [selectedNode, setSelectedNode] = useState(null);

  function aggregateNodesByDomain(nodes) {
    const domainMap = new Map();

    nodes.forEach((node) => {
      const domain = new URL(node.id).hostname;
      if (!domainMap.has(domain)) {
        domainMap.set(domain, { ...node, id: domain });
      }
    });

    return Array.from(domainMap.values());
  }

  useEffect(() => {
    const displayedNodes = viewMode === 'domain' ? aggregateNodesByDomain(nodes) : nodes;

    const svg = d3.select(svgRef.current);
    const width = svg.node().getBoundingClientRect().width;
    const height = svg.node().getBoundingClientRect().height;

    const simulation = d3
      .forceSimulation(displayedNodes)
      .force('link', d3.forceLink(links).id((d) => d.id))
      .force('charge', d3.forceManyBody().strength(-400))
      .force('center', d3.forceCenter(width / 2, height / 2));

    const link = svg
      .selectAll('.link')
      .data(links)
      .join('line')
      .attr('class', 'link')
      .attr('stroke', 'black');

    const node = svg
      .selectAll('.node')
      .data(displayedNodes)
      .join('circle')
      .attr('class', 'node')
      .attr('r', (d) => (d.crawled ? 10 : 5))
      .attr('fill', (d) => (d.crawled ? 'green' : 'red'))
      .call(d3.drag().on('start', dragstarted).on('drag', dragged).on('end', dragended));

    node.on('dblclick', (event, d) => {
      setSelectedNode(d);
    });

    simulation.on('tick', () => {
      link.attr('x1', (d) => d.source.x).attr('y1', (d) => d.source.y).attr('x2', (d) => d.target.x).attr('y2', (d) => d.target.y);

      node.attr('cx', (d) => d.x).attr('cy', (d) => d.y);
    });

    function dragstarted(event, d) {
      if (!event.active) simulation.alphaTarget(0.3).restart();
      d.fx = d.x;
      d.fy = d.y;
    }

    function dragged(event, d) {
      d.fx = event.x;
      d.fy = event.y;
    }

    function dragended(event, d) {
      if (!event.active) simulation.alphaTarget(0);
      d.fx = null;
      d.fy = null;
    }
  }, [nodes, links, viewMode]);

  return (
    <div>
      {selectedNode && (
        <NodeDetails
          node={selectedNode}
          onClose={() => setSelectedNode(null)}
        />
      )}
      <svg ref={svgRef}></svg>
    </div>
  );
}

export default VisualizationGraph;