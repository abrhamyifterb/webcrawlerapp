import { useState, useEffect } from 'react';

export const useUniqueTags = (records) => {
    const [uniqueTags, setUniqueTags] = useState([]);

    useEffect(() => {
        const tagsSet = new Set();
        records.forEach((record) => {
            record.tags.forEach((tag) => {
                tagsSet.add(tag);
            });
        });
        setUniqueTags(Array.from(tagsSet));
    }, [records]);

    return uniqueTags;
};
