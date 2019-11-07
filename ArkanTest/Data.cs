using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ArkanTest
{
    /// <summary>
     /// Базовый класс контейнера коллекции.
     /// </summary>
    public abstract class Data
    {
        /// <summary>
        /// Значения.
        /// </summary>
        public int?[] Values { get; protected set; }

        /// <summary>
        /// Количество значений в <see cref="Values"/>.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Объект синхронизации между параллельными потоками.
        /// Используется для доступа к <see cref="Values"/> и <see cref="Count"/>.
        /// </summary>
        public ReaderWriterLockSlim Lock { get; } = new ReaderWriterLockSlim();

        /// <summary>
        /// Проверка корректности.
        /// </summary>
        /// <remarks>Пример использования объекта синхронизации.</remarks>
        public bool IsValid
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Values != null
                    && Count <= Values.Length
                    && Values.Take(Count).All(v => v.HasValue);
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Инициализация массивом значений.
        /// </summary>
        /// <param name="values">Исходные значения.</param>
        public void init(int[] values)
        {
            Values = (values ?? new int[0]).Select(v => (int?)v).ToArray();
            Count = Values.Length;
        }

        /// <summary>
        /// Удаляет из коллекции значения, содержащиеся в переданном наборе.
        /// </summary>
        /// <param name="values">Значения, которые следует удалить.</param>
        /// <remarks>
        /// Этот метод следует реализовать несколькими способами.
        /// </remarks>
        public abstract void cleanup(IEnumerable<int> values);
    }



}
