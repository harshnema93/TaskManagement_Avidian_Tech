import React, { useState } from 'react';
import TaskList from './components/TaskList';
import TaskForm from './components/TaskForm';
import Search from './components/Search';
import { TaskProvider } from './context/TaskContext';
import './App.css';

function App() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editTaskId, setEditTaskId] = useState(null);


  const openForm = (taskId) => {
        setEditTaskId(taskId)
         setIsFormOpen(true)
      };


    const closeForm = () => {
           setIsFormOpen(false);
          setEditTaskId(null)
    }

return (
    <TaskProvider>
         <div className="container">
              <h1>Task Management</h1>
               <Search />
              <button onClick={() => openForm()} style={{marginBottom: "15px"}}>Add Task</button>
               {isFormOpen && <TaskForm initialTaskId={editTaskId} onClose={closeForm} />}
             <TaskList openForm={openForm} />
          </div>
     </TaskProvider>
);
}

export default App;