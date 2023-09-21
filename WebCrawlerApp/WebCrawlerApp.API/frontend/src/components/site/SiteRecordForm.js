
import React, { useEffect } from 'react';
import { SiteRecordModel } from '../../dataModels/dataModels';
import './SiteRecordForm.scss';
import { validateForm } from '../../validators/validators';
import { submitFormToAPI, updateWebsiteRecord } from '../../apis/rest/apiServices';

const SiteRecordForm = ({ onSubmit, onCancel, initialValues, onSuccess, onError }) => {
  const [formValues, setFormValues] = React.useState(initialValues || SiteRecordModel);
  const [formErrors, setFormErrors] = React.useState({});
  const [touchedFields, setTouchedFields] = React.useState({});

  const handleChange = (event) => {
      let value = event.target.value;

      if (event.target.name === "isActive") {
          value = value === "true" ? true : value === "false" ? false : value;
      }
      const updatedValues = { ...formValues, [event.target.name]: value };
      setFormValues(updatedValues);
      setTouchedFields({ ...touchedFields, [event.target.name]: true });
      setFormErrors(validateForm(updatedValues));
  };

  const handleBlur = (event) => {
      setTouchedFields({ ...touchedFields, [event.target.name]: true });
      setFormErrors(validateForm(formValues));
  };

  const handleSubmit = async (event) => {
      event.preventDefault();
      const errors = validateForm(formValues);
      let response = Object.create({});
      if (Object.keys(errors).length === 0) {
          try {
              if (initialValues && initialValues.id) {
                response = await updateWebsiteRecord(initialValues.id, formValues);
                console.log(response.Object);
              } else {
                response = await submitFormToAPI(formValues);
                console.log(response.Object);
              }

              onSubmit(formValues);
              if (onSuccess) onSuccess(response);
          } catch (error) {
              console.error("API submission error:", error);
              if (onError) onError(error);
          }
      } else {
          setFormErrors(errors);
          setTouchedFields({
              label: true,
              url: true,
              boundaryRegExp: true,
              crawlFrequency: true,
              tags: true,
              isActive: true
          });
      }
  };
  
  useEffect(() => {
    setFormValues(initialValues || SiteRecordModel);
  }, [initialValues]);

  return (
    <div className="form-container">
      <h3>{initialValues && initialValues.id ? 'Edit' : 'Add'} Site Record</h3>
      <form onSubmit={handleSubmit}>
        <label htmlFor="label">Label</label>
        <input
          type="text"
          name="label"
          id="label"
          placeholder='user given label'
          value={formValues.label || ''}
          onChange={handleChange}
          onBlur={handleBlur}
        />
        {touchedFields.label && formErrors.label && <span className="error">{formErrors.label}</span>}

        <label htmlFor="url">URL</label>
        <input
          type="text"
          name="url"
          id="url"
          placeholder='where the crawler should start'
          value={formValues.url || ''}
          onChange={handleChange}
          onBlur={handleBlur}
        />
        {touchedFields.url && formErrors.url && <span className="error">{formErrors.url}</span>}

        <label htmlFor="boundaryRegExp">RegExp Boundary</label>
        <input
          type="text"
          name="boundaryRegExp"
          id="boundaryRegExp"
          placeholder='link must match this expression in order to be followed'
          value={formValues.boundaryRegExp || ''}
          onChange={handleChange}
          onBlur={handleBlur}
        />
        {touchedFields.boundaryRegExp && formErrors.boundaryRegExp && <span className="error">{formErrors.boundaryRegExp}</span>}

        <label htmlFor="crawlFrequency">Periodicity</label>
        <input
          type="number"
          name="crawlFrequency"
          id="crawlFrequency"
          placeholder='how often should the site be crawled in terms of minutes'
          value={formValues.crawlFrequency || ''}
          onChange={handleChange}
          onBlur={handleBlur}
        />
        {touchedFields.crawlFrequency && formErrors.crawlFrequency && <span className="error">{formErrors.crawlFrequency}</span>}

        <label htmlFor="tags">Tags</label>
        <input
          type="text"
          name="tags"
          id="tags"
          placeholder='user given comma separated strings'
          value={(formValues.tags && formValues.tags.join(', ')) || ''}
          onChange={(event) => {
            const tagsArray = event.target.value.split(',').map((tag) => tag.trim());
            setFormValues({ ...formValues, tags: tagsArray });
          }}
        />

        <label htmlFor="isActive">Is Active</label>
        <select
          name="isActive"
          id="isActive"
          value={formValues.isActive}
          onChange={handleChange}
        >
          <option value="">Select</option>
          <option value={true}>Active</option>
          <option value={false}>Inactive</option>
        </select>

        <button className="save" type="submit">{initialValues && initialValues.id ? 'Update' : 'Save'}</button>
        <button type="button" onClick={onCancel}>
          Cancel
        </button>
      </form>
    </div>
  );
};

export default SiteRecordForm;