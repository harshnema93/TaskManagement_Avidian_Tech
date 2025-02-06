import React, { useContext } from 'react';
        import TaskCard from './TaskCard';
        import { TaskContext } from '../context/TaskContext';
         import Loading from './Loading';
        import ErrorMessage from './ErrorMessage';


        const TaskList = ({ openForm }) => {
        const { tasks, loading, error, editTask, removeTask } = useContext(TaskContext);

           const handleTaskEdit = (id) => {
           openForm(id)
           };

        const handleTaskDelete = async (id) => {
            await removeTask(id);
         };

            const handleTaskComplete = async (id) => {
                   const task = tasks.find(task => task.id === id);
                    if (task)
                    {
                       const updatedTask = { ...task, isCompleted: !task.isCompleted}
                         await editTask(id, updatedTask)
                    }
             }


           if (loading) {
            return <Loading/>
         }
         if (error) {
              return  <ErrorMessage message={error}/>;
        }

          if (tasks.length === 0) {
            return <p>No tasks found.</p>;
          }
            return (
                <div className="task-list">
                {tasks.map(task => (
                <TaskCard key={task.id}
                       task={task}
                     onEdit={handleTaskEdit}
                     onDelete={handleTaskDelete}
                    onComplete={handleTaskComplete}
                    />
               ))}
                </div>
            );
        };

        export default TaskList;