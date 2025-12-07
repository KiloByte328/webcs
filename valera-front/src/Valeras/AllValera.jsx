import axios from 'axios';
import React, { useEffect, useState } from 'react';
import ValeraStats from './ValeraStats';

const api = axios.create({baseURL:"https://localhost:63627/api"});

api.interceptors.request.use( (config) => {
  const token = localStorage.getItem('jwtToken');
  
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
  },
  (error) => {return Promise.reject(error);}
) 
 
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response && (error.response.status === 301 || error.response.status === 302)) {
      const redirectUrl = error.response.headers["location"];
      if (redirectUrl) {
        const token = localStorage.getItem("jwtToken");
        return api({
          method: error.config.method,
          url: redirectUrl,
          headers: { ...error.config.headers, Authorization: `Bearer ${token}` },
          data: error.config.data
        });
      }
    }
    return Promise.reject(error);
  }
);

async function get_all_valeras() {
  return api.get('/valera/AllValeras').then(res => res.data);
}

async function get_valera_by_id(id) {
  return api.get('/valera/' + id).then(res => res.data);
}

async function valera_works(id) {
  return api.post('/valera/' + id + '/work/').then(res => res.data);
}

async function valera_drink(id) {
  return api.post('/valera/' + id + '/drink/').then(res => res.data);
}

async function valera_sleep(id) {
  return api.post('/valera/' + id + '/sleep/').then(res => res.data);
}

async function valera_sing(id) {
  return api.post('/valera/' + id + '/sing_in_metro/').then(res => res.data);
}  

async function valera_bar(id) {
  return api.post('/valera/' + id + '/go_to_pub/').then(res => res.data);
}

async function valera_touch_grass(id) {
  return api.post('/valera/' + id + '/touch_grass/').then(res => res.data);
}

async function valera_friends(id) {
  return api.post('/valera/' + id + '/go_to_drink_with/').then(res => res.data);
}

async function valera_cinema(id) {
  return api.post('/valera/' + id + '/cinema/').then(res => res.data);
}

async function delete_valera(id) {
  return api.delete('/valera/' + id).then(res => res.data);
}


function ValeraList() {
  const [valeras, setValeras] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedValeraId, setSelectedValeraId] = useState(null); 

  const fetchData = async () => {
    try {
        const data = await get_all_valeras();
        const sortedData = data.sort((a, b) => a.id - b.id); 
        setValeras(sortedData);
    } catch (err) {
        setError(err.message);
    } finally {
        setLoading(false);
    }
  };

  const handleValeraDeleted = () => {
      setSelectedValeraId(null);
      fetchData(); // Перезагружаем список, чтобы удаленный Валера исчез
  }; 

  useEffect(() => {
  fetchData();

  // обновлять когда кто-то создаёт нового Валеру
  const handler = () => fetchData();
  window.addEventListener("valera:updated", handler);

  return () => window.removeEventListener("valera:updated", handler);
}, []);

  if (loading) return <div>Загрузка...</div>;
  else if (error && error !== "404") return <div>Ошибка при получении Валер: {error}</div>;

  const handleValeraSelect = (id) => {
      setSelectedValeraId(id); 
  };
  
  const handleBackClick = () => {
      setSelectedValeraId(null);
  };

  // const HandleLogOut= () => {
  //   localStorage.setItem('jwtToken', '');
  //   useNavigatr('/login')
  // }

  const showNoValerasMessage = valeras.length === 0 && !loading;

  return (
      <div> 
        {/* <div>
        <button onClick={HandleLogOut}>Разлогиниться?</button>
        </div> */}
          <h2>Список Валер:</h2>

          {selectedValeraId !== null ? (
              <div>
                  <button onClick={handleBackClick} style={{ marginBottom: '20px' }}>
                      ← Назад к списку
                  </button>
                  {/* Передаем функцию для возврата к списку после удаления */}
                  <ValeraStats 
                      id={selectedValeraId} 
                      onBack={handleBackClick} 
                      onDeleted={handleValeraDeleted}
                  /> 
              </div>

          ) : (
              showNoValerasMessage ? (
                <div>Нет Валер 😢</div>
              ) : (
                valeras.map((v) => (
                    <div 
                        key={v.id} 
                        className="valera-card" 
                        style={{border: "1px solid black", margin: "10px", padding: "10px"}}>
                        <button onClick={() => handleValeraSelect(v.id)}> 
                            <strong>{v.name}</strong> — жив: {v.is_alive?  "Да" : "Нет"}
                        </button>
                    </div>
                ))
              )
          )}
      </div>
  );
}

const fetchValeras = () => get_all_valeras();
const handleValeraCreated = () => { fetchValeras(); };

export { 
    get_all_valeras, get_valera_by_id, valera_works, valera_sleep, valera_drink, 
    valera_sing, valera_touch_grass, valera_cinema, valera_friends, valera_bar, 
    fetchValeras, handleValeraCreated, delete_valera, api
};
export default ValeraList;