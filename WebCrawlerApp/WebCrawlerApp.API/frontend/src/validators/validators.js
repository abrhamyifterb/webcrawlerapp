export const validateForm = (values) => {
    const errors = {};

    if (!values.label) {
        errors.label = 'Label is required.';
    }

    const urlPattern = new RegExp('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?');  

    if (!values.url) {
        errors.url = 'URL is required.';
    } else if (!urlPattern.test(values.url)) {
        errors.url = 'URL is not valid.';
    }

    if (!values.boundaryRegExp) {
        errors.boundaryRegExp = 'RegExp Boundary is required.';
    } else {
        try {
            new RegExp(values.boundaryRegExp);
        } catch (e) {
            errors.boundaryRegExp = 'RegExp Boundary is not valid.';
        }
    }

    if (values.crawlFrequency === undefined || values.crawlFrequency === null || values.crawlFrequency === '') {
        console.log(values.crawlFrequency);
        errors.crawlFrequency = 'Periodicity is required and should be a number.';
    }  else if (Number(values.crawlFrequency) <= 0) {
        errors.crawlFrequency = 'Periodicity should be a number greater than 0.';
    }

    return errors;
};
