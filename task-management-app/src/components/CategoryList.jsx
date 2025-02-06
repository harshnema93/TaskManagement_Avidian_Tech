import React from 'react';

        const CategoryList = ({ categories, selectedCategory, onCategoryChange }) => {

             const handleCategoryChange = (e) => {
                 onCategoryChange(parseInt(e.target.value));
                }
            return (
                <select value={selectedCategory || ''} onChange={handleCategoryChange}>
                    <option value="">Select a Category</option>
                {categories.map(category => (
                      <option key={category.id} value={category.id}>{category.name}</option>
                  ))}
                </select>
            );
        };

        export default CategoryList;