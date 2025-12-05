import { useEffect, useState } from 'react';
//import { useParams } from 'react-router-dom';
import { get_valera_by_id, valera_works, valera_sleep, valera_sing, 
    valera_touch_grass, valera_cinema, valera_friends, valera_bar, delete_valera } from './AllValera.jsx';

function ValeraStats({ id, onDeleted }) {
  const [valera, setValera] = useState(null);

  const loadValera = async () => {
    try {
      const data = await get_valera_by_id(id); // <-- используем пропс id
      setValera(data); // Убедитесь, что здесь setValera(data), а не setValera(response.data)
    } catch (error) {
      console.error('Ошибка при загрузке Валеры:', error);
    } 
  };

  useEffect(() => {
    console.log('ValeraStats: Загрузка статистики для ID:', id);
    loadValera();
  }, [id]);

  const handleDelete = async () => {
      try {
          await delete_valera(id); 
          
          if (onDeleted) {
              onDeleted();
          }
      } catch (error) {
          console.error('Ошибка при удалении Валеры:', error);
      }
    };

  if (!valera) return <div>Загрузка статистики для ID: {id}...</div>;

  return (
    <div>
      <h2>{valera.name}</h2>

      <div>Здоровье: {valera.hp}</div>
      <div>Алкоголь: {valera.mp}</div>
      <div>Жизнерадостность: {valera.cf}</div>
      <div>Усталость: {valera.ft}</div>
      <div>Деньги: {valera.mn}</div>

      {valera.is_alive ? (
      <div>
        <button onClick={async () => {await valera_works(valera.id); await loadValera()}
                  } disabled={valera.mp >= 50 || valera.ft >= 10}>Пойти на работу</button>
        <button onClick={async () => {await valera_touch_grass(valera.id)
          await loadValera();
        }}>Созерцать природу</button>
        <button onClick={async () => {await valera_cinema(valera.id); await loadValera()
        }}>Пить вино и смотреть Cinema</button>
        <button onClick={async () => {await valera_bar(valera.id); await loadValera()
        }}>Сходить в бар</button>
        <button onClick={async () => {await valera_friends(valera.id); await loadValera()
        }} >Выпить с маргинальными личностями</button>
        <button onClick={async () => {await valera_sing(valera.id); await loadValera()
        }}>Петь в метро</button>
        <button onClick={async () => {await valera_sleep(valera.id); await loadValera()
        }}>Спать</button>
      </div>
      ) : (
        <div> Ваш Валера мёртв</div>
      )}
      <button onClick={handleDelete}>Удалить Валеру?</button>
    </div>
  );
}

export default ValeraStats;
