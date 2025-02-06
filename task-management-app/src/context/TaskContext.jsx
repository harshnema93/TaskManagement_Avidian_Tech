import React, { createContext, useState, useEffect } from 'react';
import { fetchTasks, fetchCategories,createTask, updateTask, deleteTask, createCategory } from '../api';

export const TaskContext = createContext();

export const TaskProvider = ({ children }) => {
     const [tasks, setTasks] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
      const [searchTerm, setSearchTerm] = useState('');
    const [selectedCategory, setSelectedCategory] = useState('');
     const [selectedPriority, setSelectedPriority] = useState('');


const loadTasks = async () => {
  setLoading(true);
  setError(null);

    try {
    const tasksData = await fetchTasks(searchTerm, selectedCategory, selectedPriority);
    setTasks(tasksData);
    }
    catch (error)
    {
      setError(error.message)
    }
  finally {
      setLoading(false);
  }
 };


const loadCategories = async () => {
     setLoading(true);
     setError(null)
     try {
            const categoriesData = await fetchCategories();
         setCategories(categoriesData)
      }
      catch (error)
      {
          setError(error.message)
        }
    finally{
        setLoading(false)
    }
};

const addTask = async (newTask) => {
    setLoading(true);
    setError(null)

     try{
          await createTask(newTask);
          await loadTasks();

     }
    catch(error){
         setError(error.message)

     }
    finally{
         setLoading(false)
    }
};

const editTask = async (id, updatedTask) => {
     setLoading(true);
       setError(null);
    try {
        await updateTask(id, updatedTask)
        await loadTasks();
    }
     catch(error)
     {
        setError(error.message)
    }
    finally {
         setLoading(false);
     }
};


 const removeTask = async (id) => {
     setLoading(true);
       setError(null);
  try {
        await deleteTask(id);
        await loadTasks();
  }
  catch (error) {
    setError(error.message);
  }
  finally{
      setLoading(false)
  }
};


 const addCategory = async(newCategory) => {
     setLoading(true);
    setError(null);
     try{
         await createCategory(newCategory);
         await loadCategories();
     }
     catch(error) {
         setError(error.message);

     }
     finally{
        setLoading(false);
     }
 }

   const handleSearchChange = (e) => {
      setSearchTerm(e.target.value)
      };

   const handleCategoryChange = (e) => {
       setSelectedCategory(e.target.value)
      };

   const handlePriorityChange = (e) => {
       setSelectedPriority(e.target.value)
       };


    useEffect(() => {
        loadTasks();
         loadCategories();
    }, [searchTerm, selectedCategory, selectedPriority]);


    return (
        <TaskContext.Provider value={{
            tasks,
            categories,
             loading,
            error,
             addTask,
            editTask,
             removeTask,
            addCategory,
             handleSearchChange,
            handleCategoryChange,
            handlePriorityChange,
             searchTerm,
            selectedCategory,
            selectedPriority,

        }}>
            {children}
        </TaskContext.Provider>
    );
};
