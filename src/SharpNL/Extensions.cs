﻿// 
//  Copyright 2014 Gustavo J Knuppe (https://github.com/knuppe)
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// 
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//   - May you do good and not evil.                                         -
//   - May you find forgiveness for yourself and forgive others.             -
//   - May you share freely, never taking more than you give.                -
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//  

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using System.Collections.Generic;
using System.Security.Permissions;

namespace SharpNL {
    /// <summary>
    /// Utility class that makes my life easier. =D
    /// </summary>
    internal static class Extensions {

        #region . Add .

        // does not support ref in the array argument :(

        public static T[] Add<T>(this T[] array, T value) {           
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = value;
            return array;
        }
        #endregion

        #region . AddRange .

        /// <summary>
        /// Copies the contents of another <see cref="IDictionary{K, V}" /> object to the end of the collection.
        /// </summary>
        /// <typeparam name="K">The dictionary key type.</typeparam>
        /// <typeparam name="V">The dictionary value type.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="values">A <see cref="IDictionary{K, V}" /> that contains the objects to add to the collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="dictionary" />
        /// or
        /// <paramref name="values" /></exception>
        /// <exception cref="System.ArgumentException">The dictionary is read-only.</exception>
        public static void AddRange<K, V>(this IDictionary<K, V> dictionary, IDictionary<K, V> values) {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (dictionary.IsReadOnly)
                throw new ArgumentException("The dictionary is read-only.");

            if (values == null)
                throw new ArgumentNullException("values");

            if (values.Count == 0)
                return;

            

            foreach (var pair in values) {
                dictionary.Add(pair.Key, pair.Value);
            }
        }

        #endregion

        #region . AreEqual .

        /// <summary>
        /// Verifies that the all the specified objects are equal.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="others">The others.</param>
        /// <returns><c>true</c> if the all the specified objects are equal, <c>false</c> otherwise.</returns>
        public static bool AreEqual<T>(this T input, params T[] others) {
            return others.All(other => input.Equals(other));
        }

        /// <summary>
        /// Verifies that the all the specified objects are equal.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="others">The others.</param>
        /// <returns><c>true</c> if the all the specified objects are equal, <c>false</c> otherwise.</returns>
        public static bool AreEqual<T>(this T[] input, params T[][] others) {
            return others.All(other => input.SequenceEqual(other));
        }
        #endregion

        #region . Contains .
        internal static bool Contains(this string[] array, string value) {
            return array.Any(item => item == value);
        }
        #endregion

        #region . Fill .

        /// <summary>
        /// Fills the specified input with the given value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The array.</param>
        /// <param name="value">The value.</param>
        internal static void Fill<T>(this T[] input, T value) {
            for (int i = 0; i < input.Length; i++) {
                input[i] = value;
            }
        }
        #endregion

        #region . GetKey .
        /// <summary>
        /// Gets the element key by the first element that are equals to the specified <paramref name="value"/>,
        /// of a <c>default(K)</c> value if no element is found.
        /// </summary>
        /// <typeparam name="K">The type of the key of the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="V">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>default(k)</c> if no element is found; otherwise the first element key found in the <paramref name="dictionary"/>.</returns>
        public static K GetKey<K, V>(this IDictionary<K, V> dictionary, V value) {
            return dictionary.FirstOrDefault(pair => Equals(pair.Value, value)).Key;
        }
        #endregion

        #region . Each .
        /// <example>
        /// var values = new string[] { "a", "b", "c" };
        /// strings.Each( ( value, index ) => {
        ///     // nice hack ;)
        /// });
        /// </example>
        internal static void Each<T>(this IEnumerable<T> ie, Action<T, int> action) {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }
        #endregion

        #region . ReadAllBytes .
        /// <summary>
        /// Reads the contents of the stream into a byte array, and then closes the stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>A byte array containing the contents of the stream.</returns>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        /// <exception cref="System.NotSupportedException">The stream was not readable.</exception>
        public static byte[] ReadAllBytes(this Stream stream) {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream.CanRead)
                throw new NotSupportedException("The stream was not readable.");

            var memory = stream as MemoryStream;
            if (memory != null)
                return memory.ToArray();

            using (memory = new MemoryStream()) {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }
        #endregion

        #region . SequenceEqual .
        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the default equality comparer for their type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The first enumerable.</param>
        /// <param name="second">The second enumerable.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns><c>true</c> if the two source sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">first or second.</exception>
        internal static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer = null) {
            if (first == null && second == null)
                return true;

            if (first == null) {
                throw new ArgumentNullException("first");
            }
            if (second == null) {
                throw new ArgumentNullException("second");
            }
            if (comparer == null) {
                comparer = EqualityComparer<T>.Default;
            }

            using (IEnumerator<T> enumerator = first.GetEnumerator())
            using (IEnumerator<T> enumerator2 = second.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    if (!enumerator2.MoveNext() || !comparer.Equals(enumerator.Current, enumerator2.Current)) {
                        return false;
                    }
                }
                if (enumerator2.MoveNext()) {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region . SubArray .
        public static T[] SubArray<T>(this T[] data, int index, int length) {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        #endregion

        #region + SubList .

        internal static ReadOnlyCollection<T> SubList<T>(this IReadOnlyList<T> list, int start, int count) {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            if (start > list.Count || start + count > list.Count)
                throw new ArgumentOutOfRangeException("start");

            var sub = new List<T>();
            for (int i = start; i < start + count; i++) {
                sub.Add(list[i]);
            }
            return new ReadOnlyCollection<T>(sub);
        }

        internal static List<T> SubList<T>(this IList<T> list, int start, int count) {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            if (start > list.Count || start + count > list.Count)
                throw new ArgumentOutOfRangeException("start");

            var sub = new List<T>();
            for (int i = start; i < start + count; i++) {
                sub.Add(list[i]);
            }
            return sub;
        }

        #endregion

        #region . ToArray .
        /// <summary>
        /// Copies a specific number of elements in the current stack into a new array.
        /// </summary>
        /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
        /// <param name="stack">The stack.</param>
        /// <param name="count">The number of elements to be copied into a new array.</param>
        /// <returns>A new array containing copies of the elements of the stack.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">count</exception>
        public static T[] ToArray<T>(this Stack<T> stack, int count) {

            if (stack.Count < count)
                throw new ArgumentOutOfRangeException("count");

            int index = 0;
            var array = new T[count];
            IEnumerator<T> e;
            for (e = stack.GetEnumerator(); index < count && e.MoveNext(); index++) {
                array[index] = e.Current;
            }
            return array;
        }

        public static T[] ToArray<T>(this IEnumerable<T> enumerable) {
            return new List<T>(enumerable).ToArray();
        }

        #endregion

        #region . ToDisplay .
        internal static string ToDisplay(this string[] list) {
            return string.Format("[{0}]", string.Join(", ", list));
        }
        #endregion

        #region . IEnumerator.Cast .
        public static IEnumerator<T> Cast<T>(this IEnumerator iterator) {
            while (iterator.MoveNext()) {
                yield return (T)iterator.Current;
            }
        }
        #endregion

        #region . In .
        /// <summary>
        /// Determines if the instance is equal to any of the specified values
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input object.</param>
        /// <param name="values">The values to be compared.</param>
        /// <returns><c>true</c> if the instance is equal to any of the specified values, <c>false</c> otherwise.</returns>
        public static bool In<T>(this T input, params T[] values) {
            return values.Contains(input);

        }

        /// <summary>
        /// Determines if the instance is equal to any of the specified values
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input object.</param>
        /// <param name="values">The values to be compared.</param>
        /// <returns><c>true</c> if the instance is equal to any of the specified values, <c>false</c> otherwise.</returns>
        public static bool In<T>(this T input, IEnumerable<T> values) {
            return values.Any(arg => input.Equals(arg));
        }
        #endregion

        #region . HasDefaultConstructor .
        public static bool HasDefaultConstructor(this Type t) {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }
        #endregion

        #region . GetSubclasses .
        /// <summary>
        /// Gets a list containing all subclasses for the given type.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="checkDefaultConstructor">if set to <c>true</c> will be included only the types that have a default constructor.</param>
        /// <returns>A list containing all subclasses for the given type.</returns>
        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        internal static List<Type> GetSubclasses(this Type input, bool checkDefaultConstructor) {
            var list = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                var modules = assembly.GetModules();
                foreach (var module in modules) {
                    var types = module.GetTypes();
                    foreach (var type in types) {
                        if (type.IsSubclassOf(input)) {
                            if (checkDefaultConstructor && !type.HasDefaultConstructor())
                                continue;
                            
                            list.Add(type);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        
    }
}