// using System;
// using System.Text;
// using UnityEngine;
//
// namespace SharedLibrary;
//
// public class Packet : IDisposable
// {
//     private List<byte> _buffer;
//     private byte[] _readBuffer;
//     private int _readPos;
//     private bool _bufferUpdated;
//     
//     protected bool disposed = false;
//     
//     public Packet()
//     {
//         _buffer = new List<byte>();
//         _readBuffer = Array.Empty<byte>();
//         _readPos = 0;
//     }
//
//     public Packet(byte[] data)
//     {
//         _buffer = new List<byte>();
//         _readBuffer = Array.Empty<byte>();
//         _readPos = 0;
//         
//         SetData(data);
//     }
//
//     public void SetData(byte[] data)
//     {
//         Write(data);
//         _readBuffer = _buffer.ToArray();
//     }
//
//     public byte[] ToArray()
//     {
//         return _buffer.ToArray();
//     }
//     
//     public int Length()
//     {
//         return _buffer.Count;
//     }
//
//     public int UnreadLength()
//     {
//         return Length() - _readPos;
//     }
//     
//     public void WriteLength()
//     {
//         _buffer.InsertRange(0,BitConverter.GetBytes(_buffer.Count));
//     }
//
//     public void InsertInt(int value)
//     {
//         _buffer.InsertRange(0, BitConverter.GetBytes(value));
//     }
//
//     public void Clear()
//     {
//         _buffer.Clear();
//         _readBuffer = Array.Empty<byte>();
//         _readPos = 0;
//         _bufferUpdated = false;
//     }
//     
//     #region Write
//
//     public void Write(byte data)
//     {
//         _buffer.Add(data);
//         _bufferUpdated = true;
//     }
//
//     public void Write(byte[] data)
//     {
//         _buffer.AddRange(data);
//         _bufferUpdated = true;
//     }
//
//     public void Write(int data)
//     {
//         _buffer.AddRange(BitConverter.GetBytes(data));
//         _bufferUpdated = true;
//     }
//     
//     public void Write(float data)
//     {
//         _buffer.AddRange(BitConverter.GetBytes(data));
//         _bufferUpdated = true;
//     }
//     
//     public void Write(long data)
//     {
//         _buffer.AddRange(BitConverter.GetBytes(data));
//         _bufferUpdated = true;
//     }
//     
//     public void Write(string data)
//     {
//         Write(data.Length);
//         _buffer.AddRange(Encoding.ASCII.GetBytes(data));
//         _bufferUpdated = true;
//     }
//     
//     public void Write(bool data)
//     {
//         _buffer.AddRange(BitConverter.GetBytes(data));
//         _bufferUpdated = true;
//     }
//
//     public void Write(Vector3 data)
//     {
//         Write(data.x);
//         Write(data.y);
//         Write(data.z);
//         _bufferUpdated = true;
//     }
//
//     public void Write(Quaternion data)
//     {
//         Write(data.x);
//         Write(data.y);
//         Write(data.z);
//         Write(data.w);
//         _bufferUpdated = true;
//     }
//
//     #endregion
//
//     #region Read
//
//     public byte[] ReadBytes(int length, bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             byte[] val = _buffer.GetRange(_readPos, length).ToArray();
//         
//             if (updateReadPos)
//             {
//                 _readPos += length;
//             }
//         
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public short ReadShort(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             short val = BitConverter.ToInt16(_readBuffer, _readPos);
//             
//             if (updateReadPos)
//             {
//                 _readPos += 2;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public int ReadInt(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             int val = BitConverter.ToInt32(_readBuffer, _readPos);
//             
//             if (updateReadPos)
//             {
//                 _readPos += 4;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public long ReadLong(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             long val = BitConverter.ToInt64(_readBuffer, _readPos);
//             
//             if (updateReadPos)
//             {
//                 _readPos += 8;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public float ReadFloat(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             float val = BitConverter.ToSingle(_readBuffer, _readPos);
//             
//             if (updateReadPos)
//             {
//                 _readPos += 4;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public bool ReadBool(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             bool val = BitConverter.ToBoolean(_readBuffer, _readPos);
//             
//             if (updateReadPos)
//             {
//                 _readPos += 1;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//     
//     public string ReadString(bool updateReadPos = true)
//     {
//         if (Length() > _readPos)
//         {
//             if (_bufferUpdated)
//             {
//                 _readBuffer = _buffer.ToArray();
//                 _bufferUpdated = false;
//             }
//
//             var length = ReadInt();
//             string val = Encoding.ASCII.GetString(_readBuffer, _readPos, length);
//
//             if (updateReadPos)
//             {
//                 _readPos += length;
//             }
//             
//             return val;
//         }
//         else
//         {
//             throw new Exception("Trying to read Past Limit from Packet");
//         }
//     }
//
//     public Vector3 ReadVector3(bool updateReadPos = true)
//     {
//         return new Vector3(ReadFloat(updateReadPos), ReadFloat(updateReadPos), ReadFloat(updateReadPos));
//     }
//
//     public Quaternion ReadQuaternion(bool updateReadPos = true)
//     {
//         return new Quaternion(ReadFloat(updateReadPos), ReadFloat(updateReadPos), ReadFloat(updateReadPos), ReadFloat(updateReadPos));
//     }
//     
//     #endregion
//     
//     protected virtual void Dispose(bool _disposing)
//     {
//         if (!disposed)
//         {
//             if (_disposing)
//             {
//                 _buffer.Clear();
//                 _readBuffer = Array.Empty<byte>();
//                 _readPos = 0;
//             }
//
//             disposed = true;
//         }
//     }
//     
//     public void Dispose()
//     {
//         Dispose(true);
//         GC.SuppressFinalize(this);
//     }
// }