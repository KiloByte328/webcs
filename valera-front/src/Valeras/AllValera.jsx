import axios from 'axios';
import React, { useEffect, useState } from 'react';
import ValeraStats from './ValeraStats';

axios.defaults.baseURL = 'http://localhost:63628/api';

// --- API ФУНКЦИИ (ОСТАЮТСЯ ВНЕ КОМПОНЕНТА) ---

async function get_all_valeras() {
  return axios.get('/valera/AllValeras').then(res => res.data);
}

async function get_valera_by_id(id) {
  return axios.get('/valera/' + id).then(res => res.data);
}

async function valera_works(id) {
  return axios.post('/valera/' + id + '/work/').then(res => res.data);
}

async function valera_drink(id) {
  return axios.post('/valera/' + id + '/drink/').then(res => res.data);
}

async function valera_sleep(id) {
  return axios.post('/valera/' + id + '/sleep/').then(res => res.data);
}

async function valera_sing(id) {
  return axios.post('/valera/' + id + '/sing_in_metro/').then(res => res.data);
}  

async function valera_bar(id) {
  return axios.post('/valera/' + id + '/go_to_pub/').then(res => res.data);
}

async function valera_touch_grass(id) {
  return axios.post('/valera/' + id + '/touch_grass/').then(res => res.data);
}

async function valera_friends(id) {
  return axios.post('/valera/' + id + '/go_to_drink_with/').then(res => res.data);
}

async function valera_cinema(id) {
  return axios.post('/valera/' + id + '/cinema/').then(res => res.data);
}

async function delete_valera(id) {
  return axios.delete('/valera/' + id).then(res => res.data);
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
    fetchData(); // Вызываем fetchData для первоначальной загрузки
  }, []); // Выполняется только один раз

  if (loading) return <div>Загрузка...</div>;
  else if (error && error !== "404") return <div>Ошибка при получении Валер: {error}</div>;

  const handleValeraSelect = (id) => {
      setSelectedValeraId(id); 
  };
  
  const handleBackClick = () => {
      setSelectedValeraId(null);
  };

  // Упрощение логики для отображения, когда валеры не найдены
  const showNoValerasMessage = valeras.length === 0 && !loading;

  return (
      <div> 
          <h2>Список Валер:</h2>

          {selectedValeraId !== null ? (
              // --- ЭКРАН СТАТИСТИКИ (Если выбран ID) ---
              <div>
                  <button onClick={handleBackClick} style={{ marginBottom: '20px' }}>
                      ← Назад к списку
                  </button>
                  {/* Передаем функцию для возврата к списку после удаления */}
                  <ValeraStats 
                      id={selectedValeraId} 
                      onBack={handleBackClick} 
                      onDeleted={handleValeraDeleted} // ✅ Исправлено: передаем локальную функцию
                  /> 
              </div>

          ) : (
              // --- ЭКРАН СПИСКА (Если selectedValeraId === null) ---
              
              showNoValerasMessage ? (
                  <div>Нет Валер 😢</div>
              ) : (
                  valeras.map((v) => (
                      <div 
                          key={v.id} 
                          className="valera-card" 
                          style={{border: "1px solid black", margin: "10px", padding: "10px"}}>
                          <button onClick={() => handleValeraSelect(v.id)}> 
                              <strong>{v.name}</strong> — жив: {v.is_alive?  "Да" : "Нет"} HP: {v.hp} MP: {v.mp} Усталость: {v.ft} Жизнераость: {v.cf} Деньги: {v.mn}
                          </button>
                      </div>
                  ))
              )
          )}
      </div>
  );
}

// --- ЭКСПОРТЫ ---

// Функция fetchValeras, handleValeraCreated (если она нужна где-то еще, кроме этого файла)
// Если она нужна, объявите ее как const и экспортируйте.
const fetchValeras = () => get_all_valeras();
const handleValeraCreated = () => { fetchValeras(); };

export { 
    get_all_valeras, get_valera_by_id, valera_works, valera_sleep, valera_drink, 
    valera_sing, valera_touch_grass, valera_cinema, valera_friends, valera_bar, 
    fetchValeras, handleValeraCreated, delete_valera 
};
export default ValeraList;