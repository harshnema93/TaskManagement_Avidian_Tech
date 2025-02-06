import React from 'react';

const TaskCard = ({ task, onEdit, onDelete, onComplete }) => {

    const getPriorityClass = () => {
        switch (task.priority) {
            case "High":
                return "priority-high"
             case "Medium":
                return "priority-medium"
            case "Low":
                return "priority-low"
              default:
              return "";
        }
    }


  return (
       <div className="task-card">
            <h3>{task.title}</h3>
           <p>{task.description}</p>
             {task.category && <p>Category: {task.category.name}</p>}
             <p>Priority: <span className={getPriorityClass()}>{task.priority}</span> </p>
             {task.dueDate && <p>Due Date: {new Date(task.dueDate).toLocaleDateString()}</p>}
              <p>Completed: {task.isCompleted ? "Yes" : "No"}</p>
             <div>
                <button onClick={() => onEdit(task.id)}>Edit</button>
                 <button onClick={() => onComplete(task.id)} className="completed-btn" >
                        {task.isCompleted ? 'Mark Incomplete' : 'Mark Complete'}
                </button>
               <button  onClick={() => onDelete(task.id)} className="delete-btn" >Delete</button>
            </div>
         </div>
   );
};

export default TaskCard;