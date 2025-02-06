import React, { useContext } from 'react';
        import { TaskContext } from '../context/TaskContext';

        const Search = () => {
            const { handleSearchChange, handleCategoryChange,handlePriorityChange,categories} = useContext(TaskContext);

        return (
            <div className="search-bar">
                <input type="text" placeholder="Search by title or description" onChange={handleSearchChange} />

                <select onChange={handleCategoryChange}>
                    <option value="">Select a Category</option>
                    {categories.map(category => (
                        <option key={category.id} value={category.name}>{category.name}</option>
                    ))}
                </select>
                <select onChange={handlePriorityChange}>
                    <option value="">Select a priority</option>
                     <option value="Low">Low</option>
                    <option value="Medium">Medium</option>
                    <option value="High">High</option>
                </select>
            </div>
       );
    };

    export default Search;