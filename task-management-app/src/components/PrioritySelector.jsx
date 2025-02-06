import React from 'react';

        const PrioritySelector = ({ selectedPriority, onPriorityChange }) => {
            const handleChange = (e) => {
                 onPriorityChange(e.target.value);
                }
            return (
                <select value={selectedPriority} onChange={handleChange}>
                    <option value="Low">Low</option>
                    <option value="Medium">Medium</option>
                    <option value="High">High</option>
                </select>
            );
        };

        export default PrioritySelector;