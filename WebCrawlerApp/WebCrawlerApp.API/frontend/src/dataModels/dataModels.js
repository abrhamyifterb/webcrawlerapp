export const SiteRecordModel = {
  id: null,
  url: "",
  boundaryRegExp: "",
  crawlFrequency: 0,
  label: "",
  isActive: false,
  tags: [],
  lastExecutionTime: null,
  crawledData: ""
};

const ExecutionStatus = {
  Started: 'Started',
  Completed: 'Completed',
  Failed: 'Failed',
};

export const ExecutionRecordModel = {
  id: null,
  websiteId: null,
  websiteLabel: '',
  status: null,
  startTime: null,
  endTime: null,
  crawledSitesCount: 0,
};



export const SearchCriteriaModel = {
    searchUrl: '',
    searchRegEXBoundary: '',
    searchLabel: '',
    searchPeriodicity: '',
    searchActiveStatus: null,
    searchTags: []
  };
  
export const SortConfigModel = {
    field: 'id',
    order: 'asc'
  };