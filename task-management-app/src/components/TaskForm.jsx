import React, { useState, useContext, useEffect } from 'react';
    import { TaskContext } from '../context/TaskContext';
    import CategoryList from './CategoryList';
    import PrioritySelector from './PrioritySelector';
    import { fetchTaskById } from '../api';

    const TaskForm = ({ initialTaskId, onClose}) => {
         const { addTask, editTask, categories } = useContext(TaskContext);

      const [task, setTask] = useState({
            title: '',
            description: '',
            categoryId: '',
            priority: 'Low',
            dueDate: null,
            isCompleted: false
          });

      const [formTitle, setFormTitle] = useState("Add Task");
       const [loading, setLoading] = useState(false);
       const [error, setError] = useState(null);



    useEffect(()=> {
            if(initialTaskId) {
               setFormTitle("Edit Task");
                loadTaskDetails();
            }
            else{
                 setFormTitle("Add Task");
                 setTask({
                      title: '',
                    description: '',
                   categoryId: '',
                    priority: 'Low',
                   dueDate: null,
                   isCompleted: false
                  });
            }
    },[initialTaskId])

    const loadTaskDetails = async () => {
            setLoading(true);
            setError(null);
        try{
                const taskDetails = await fetchTaskById(initialTaskId);
               if (taskDetails)
               {
                   const dueDate = taskDetails.dueDate ? new Date(taskDetails.dueDate).toISOString().split('T')[0] : null
                    setTask({
                       title: taskDetails.title,
                       description: taskDetails.description,
                       categoryId: taskDetails.categoryId,
                       priority: taskDetails.priority,
                       dueDate: dueDate,
                        isCompleted: taskDetails.isCompleted,

                   })
                }
        }
       catch(error)
       {
         setError(error.message);
       }
        finally {
          setLoading(false)
        }
    }

        const handleChange = (e) => {
            const { name, value } = e.target;
            setTask((prevTask) => ({ ...prevTask, [name]: value }));
            };

         const handleCategoryChange = (categoryId) => {
             setTask((prevTask) => ({ ...prevTask, categoryId: categoryId }));
            };

       const handlePriorityChange = (priority) => {
            setTask((prevTask) => ({ ...prevTask, priority: priority }));
            };


         const handleSubmit = async (e) => {
                e.preventDefault();
                if(initialTaskId) {
                      await editTask(initialTaskId, task)
                }
                else {
                   await addTask(task);
                }
                onClose();
            };


        if (loading)
        {
          return <p>Loading Form...</p>
        }
       if (error) {
           return <p>{error}</p>
       }

            return (
                <div className="task-form">
                    <h2>{formTitle}</h2>
                    <form onSubmit={handleSubmit}>
                         <div className="form-group">
                            <label>Title:</label>
                            <input type="text" name="title" value={task.title} onChange={handleChange} required/>
                         </div>

                         <div className="form-group">
                            <label>Description:</label>
                             <textarea name="description" value={task.description || ""} onChange={handleChange} />
                        </div>

                        <div className="form-group">
                            <label>Category:</label>
                            <CategoryList categories={categories} selectedCategory={task.categoryId} onCategoryChange={handleCategoryChange}/>
                        </div>
                          <div className="form-group">
                            <label>Priority:</label>
                             <PrioritySelector  selectedPriority={task.priority} onPriorityChange={handlePriorityChange}/>
                          </div>
                          <div className="form-group">
                             <label>Due Date:</label>
                             <input type="date" name="dueDate" value={task.dueDate || ''} onChange={handleChange} />
                         </div>


                        <button type="submit"> {initialTaskId ? "Update" : "Add"} Task</button>
                        <button type="button" onClick={onClose}>Cancel</button>
                    </form>
               </div>
            );
        };

        export default TaskForm;