import gql from 'graphql-tag';

export const FETCH_WEBSITES = gql`
  query {
    websites {
      identifier
      label
      url
      regexp
      tags
      active
    }
  }
`;

export const FETCH_NODES = gql`
  query Nodes($webPages: [UUID!]!) {
    nodes(webPages: $webPages) {
      title
      url
      crawlTime
      links {
        url
      }
      owner {
        identifier
        label
        url
        regexp
        tags
        active
      }
    }
  }
`;
