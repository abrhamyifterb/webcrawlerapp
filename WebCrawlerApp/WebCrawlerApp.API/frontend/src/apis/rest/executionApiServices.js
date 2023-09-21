const API_ENDPOINT = process.env.REACT_APP_API_ENDPOINT;

export const fetchExecutionRecord = async (recordId) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Execution/${recordId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};

export const fetchAllExecutionRecords = async () => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Execution`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};

export const fetchExecutionRecordByWebsiteId = async (recordId) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Execution/website/${recordId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};


export const fetchExecutionRecordsByWebsiteIds = async (recordIds) => {
    if (!recordIds || recordIds.length === 0) {
        console.warn('No recordIds provided. Skipping fetch request.');
        return null;
    }
    try {
        const queryString = recordIds.map(id => `websiteIds=${id}`).join('&');
        const response = await fetch(`${API_ENDPOINT}/Execution/websites/latest?${queryString}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            return await response.json(); 
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};


