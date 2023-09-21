const API_ENDPOINT = process.env.REACT_APP_API_ENDPOINT;

export const submitFormToAPI = async (data) => {
    try {
        
        const response = await fetch(`${API_ENDPOINT}/Website`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (response.ok) {
            const responseData = await response.json();
            console.log('Success:', responseData);
            return responseData;
        } else {
            console.error('Failed:', response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};

export const fetchWebsiteRecord = async (recordId) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Website/${recordId}`, {
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

export const fetchAllWebsiteRecords = async () => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Website`, {
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

export const updateWebsiteRecord = async (recordId, data) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Website/${recordId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
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

export const crawlWebsiteRecord = async (recordId) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Website/${recordId}/crawl`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            console.log('Record successfully crawled');
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};

export const deleteWebsiteRecord = async (recordId) => {
    try {
        const response = await fetch(`${API_ENDPOINT}/Website/${recordId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            console.log('Record successfully deleted');
        } else {
            console.error('Failed:', response.statusText);
            throw new Error(response.statusText);
        }
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};
