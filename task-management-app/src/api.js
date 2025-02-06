import axios from 'axios';

const API_BASE_URL = 'http://localhost:5063/api'; // Replace with your backend URL

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
    'Content-Type': 'application/json',
}
});

export const fetchTasks = async (searchTerm, category, priority) => {
    try {
        const params = new URLSearchParams();
        if (searchTerm) params.append('searchTerm', searchTerm);
        if (category) params.append('category', category);
        if (priority) params.append('priority', priority);
        const response = await api.get(`/tasks?${params.toString()}`);

        return response.data;
    } catch (error) {
        console.error("Error fetching tasks:", error);
        throw error;
    }
};


export const fetchTaskById = async (id) => {
    try{
        const response = await api.get(`/tasks/${id}`)
        return response.data
    }catch(error){
         console.error(`Error fetching task with ID ${id}:`, error);
         throw error;
    }
}

export const createTask = async (task) => {
  try {
    const response = await api.post('/tasks', task);
    return response.data;
   } catch (error) {
      console.error('Error creating task:', error);
        throw error;
    }
};

export const updateTask = async (id, task) => {
    try {
    const response = await api.put(`/tasks/${id}`, task);
    return response.data;
    } catch (error) {
    console.error(`Error updating task with ID ${id}:`, error);
        throw error;
    }
};

export const deleteTask = async (id) => {
    try {
         await api.delete(`/tasks/${id}`);
    } catch (error) {
        console.error(`Error deleting task with ID ${id}:`, error);
         throw error;
    }
};

export const fetchCategories = async () => {
    try {
      const response = await api.get('/categories');
      return response.data;
    } catch (error) {
       console.error('Error fetching categories:', error);
        throw error;
    }
};

export const createCategory = async (category) => {
    try {
        const response = await api.post('/categories', category)
        return response.data
    }
    catch(error)
    {
        console.error('Error creating category', error)
        throw error;
    }
}