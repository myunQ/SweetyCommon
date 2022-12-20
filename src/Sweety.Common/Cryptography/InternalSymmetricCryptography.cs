/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  对称加密/解密的实现
 * 
 * 
 * Members Index:
 *      class
 *          InternalSymmetricCryptography   
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Cryptography
{
    using System;
    using System.IO;
    using System.Text;
    using System.Security.Cryptography;


    /// <summary>
    /// 对称加密/解密。
    /// </summary>
    internal class InternalSymmetricCryptography : ISymmetricCryptography
    {
        /// <summary>
        /// 加密算法对象实例。
        /// </summary>
        private SymmetricAlgorithm _cryptograph = null;
        private string _algorithmName = null;

        /// <summary>
        /// 创建一个对称加密算法的实例。
        /// </summary>
        /// <param name="cryptograph">实际执行对称加密算法的实例。</param>
        /// <exception cref="System.ArgumentNullException">当 <paramref name="cryptograph"/> 参数为 <c>null</c> 引用时引发。</exception>
        internal InternalSymmetricCryptography(SymmetricAlgorithm cryptograph)
        {
            if (cryptograph == null) throw new ArgumentNullException(nameof(cryptograph));

            this._cryptograph = cryptograph;
        }
        /// <summary>
        /// 创建一个对称加密算法的实例。
        /// </summary>
        /// <param name="cryptograph">实际执行对称加密算法的实例。</param>
        /// <param name="key">对称加密算法使用的密钥。</param>
        /// <param name="iv">对称加密算法使用的初始化向量。</param>
        /// <exception cref="System.ArgumentNullException">当 <paramref name="cryptograph"/>、<paramref name="key"/>、<paramref name="iv"/> 参数为 <c>null</c> 引用时引发。</exception>
        internal InternalSymmetricCryptography(SymmetricAlgorithm cryptograph, byte[] key, byte[] iv)
            : this(cryptograph)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (iv == null) throw new ArgumentNullException(nameof(iv));

            this.Key = key;
            this.IV = iv;
        }
        /// <summary>
        /// 创建一个对称加密算法的实例。
        /// </summary>
        /// <param name="cryptograph">实际执行对称加密算法的实例。</param>
        /// <param name="key">对称加密算法使用的密钥。</param>
        /// <param name="iv">对称加密算法使用的初始化向量。</param>
        /// <param name="mode">对称算法的运算模式。</param>
        /// <param name="padding">对称算法中使用的填充模式。</param>
        /// <exception cref="System.ArgumentNullException">当 <paramref name="cryptograph"/>、<paramref name="key"/>、<paramref name="iv"/> 参数为 <c>null</c> 引用时引发。</exception>
        internal InternalSymmetricCryptography(SymmetricAlgorithm cryptograph, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding)
            : this(cryptograph, key, iv)
        {
            this.Mode = mode;
            this.Padding = padding;
        }


        #region ISymmetricCryptography interface implementation.
        public string AlgorithmName
        {
            get
            {
                if (this._algorithmName == null)
                {
                    this._algorithmName = this._cryptograph.GetType().Name;
                }
                return this._algorithmName;
            }
        }

        public byte[] Key
        {
            get => _cryptograph.Key;
            
            set
            {
                if (value == null) throw new ArgumentNullException();

                int keySize = value.Length * 8;
                if (keySize != this._cryptograph.KeySize)
                {
                    if (this._cryptograph.LegalKeySizes.Length != 1)
                    {
                        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
                         * 目前观察 .net 提供的各类对称加密算法的 LegalKeySizes 属性
                         * 和 LegalBlockSizes 属性均仅包含一个元素。
                         * 在此处设置异常，一旦异常抛出则需要重构该属性计算 KeySize 值的代码。
                         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                        throw new InvalidOperationException(String.Format(Properties.Localization.the_XXX_property_of_the_encryption_algorithm_instance_contains_XXX_elements__not_just_one_element, nameof(_cryptograph.LegalKeySizes), _cryptograph.LegalKeySizes.Length));
                    }

                    // 当 KeySizes.SkipSize 值为零时，其 MaxSize 与 MinSize 相等。
                    KeySizes keySizes = this._cryptograph.LegalKeySizes[0];
                    if (keySizes.MinSize > keySize || keySizes.MaxSize < keySize
                        || (keySize % keySizes.SkipSize) != 0
                        || (keySizes.SkipSize == 0 && keySizes.MaxSize != keySize))
                    {
                        throw new CryptographicException(Properties.Localization.the_specified_key_size_is_invalid_for_this_algorithm);
                    }

                    this._cryptograph.KeySize = keySize;
                }

                this._cryptograph.Key = value;
            }
        }

        public byte[] IV
        {
            get => _cryptograph.IV;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                int blockSize = value.Length * 8;
                if (blockSize != this._cryptograph.BlockSize)
                {
                    if (this._cryptograph.LegalBlockSizes.Length != 1)
                    {
                        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
                         * 目前观察 .net 提供的各类对称加密算法的 LegalBlockSizes 属性
                         * 和 LegalKeySizes 属性均仅包含一个元素。
                         * 在此处设置异常，一旦异常抛出则需要重构该属性计算 BlockSize 值的代码。
                         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                        throw new InvalidOperationException(String.Format(Properties.Localization.the_XXX_property_of_the_encryption_algorithm_instance_contains_XXX_elements__not_just_one_element, nameof(_cryptograph.LegalBlockSizes), _cryptograph.LegalBlockSizes.Length));
                    }

                    // 当 SkipSize 值为零时，其 MaxSize 与 MinSize 相等。
                    var ivSizes = this._cryptograph.LegalBlockSizes[0];
                    if (ivSizes.MinSize > blockSize || ivSizes.MaxSize < blockSize
                        || (blockSize % ivSizes.SkipSize) != 0
                        || (ivSizes.SkipSize == 0 && ivSizes.MaxSize != blockSize))
                    {
                        throw new CryptographicException(Properties.Localization.the_specified_initialization_vector_does_not_match_the_block_size_of_this_algorithm);
                    }

                    this._cryptograph.BlockSize = blockSize;
                }

                this._cryptograph.IV = value;
            }
        }

        public CipherMode Mode
        {
            get => _cryptograph.Mode;
            set
            {
                if (!Enum.IsDefined(typeof(CipherMode), value)) throw new ArgumentException(Properties.Localization.unknown_encryption_cipher_mode);

                this._cryptograph.Mode = value;
            }
        }

        public PaddingMode Padding
        {
            get => _cryptograph.Padding;
            set
            {
                if (!Enum.IsDefined(typeof(PaddingMode), value)) throw new ArgumentException(Properties.Localization.unknown_data_block_padding_mode);

                this._cryptograph.Padding = value;
            }
        }

#if !NETSTANDARD2_0
        public byte[] Encrypt(ReadOnlySpan<byte> plaintext)
        {
            if (plaintext.IsEmpty) throw new ArgumentException(Properties.Localization.the_plaintext_cannot_be_empty);

            using (ICryptoTransform encryptor = this._cryptograph.CreateEncryptor())
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                    {
                        cStream.Write(plaintext);
                        //这里要是不使用 using (...) { ... } 的话就要在调用 mStream.ToArray()
                        //之前，先调用 cStream.FlushFinalBlock()。否则 mStream 里的数据不全。
                    }

                    byte[] encrypted = mStream.ToArray();
                    return encrypted;
                }
            }
        }
#endif

        public byte[] Encrypt(byte[] plaintext)
        {
            if (plaintext == null) throw new ArgumentNullException(nameof(plaintext));
            if (plaintext.Length == 0) throw new ArgumentException(Properties.Localization.the_plaintext_cannot_be_empty);

            using (ICryptoTransform encryptor = this._cryptograph.CreateEncryptor())
            {
                byte[] encrypted = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
                return encrypted;
            }
        }

#if !NETSTANDARD2_0
        public byte[] Encrypt(Encoding encoding, ReadOnlySpan<char> plaintext)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (plaintext.IsEmpty) throw new ArgumentException(Properties.Localization.the_plaintext_cannot_be_empty);
#if SPACE
            Span<byte> bytes = stackalloc byte[encoding.GetByteCount(plaintext)];
            _ = encoding.GetBytes(plaintext, bytes);
            return Encrypt(bytes);
#else
            Span<byte> bytes = stackalloc byte[encoding.GetMaxByteCount(plaintext.Length)];
            int count = encoding.GetBytes(plaintext, bytes);
            return Encrypt(bytes.Slice(0, count));
#endif
        }
#endif

        public byte[] Encrypt(Encoding encoding, char[] plaintext)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (plaintext == null) throw new ArgumentNullException(nameof(plaintext));
            if (plaintext.Length == 0) throw new ArgumentException(Properties.Localization.the_plaintext_cannot_be_empty);

            return Encrypt(encoding.GetBytes(plaintext));
        }

        public byte[] Encrypt(Encoding encoding, string plaintext)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (plaintext == null) throw new ArgumentNullException(nameof(plaintext));
            if (plaintext.Length == 0) throw new ArgumentException(Properties.Localization.the_plaintext_cannot_be_empty);

            return Encrypt(encoding.GetBytes(plaintext));
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            if (ciphertext == null) throw new ArgumentNullException(nameof(ciphertext));
            if (ciphertext.Length == 0) throw new ArgumentException(Properties.Localization.the_ciphertext_cannot_be_empty);

            using (ICryptoTransform decryptor = this._cryptograph.CreateDecryptor())
            {
                byte[] decrypted = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                return decrypted;
            }
        }

        public byte[] Decrypt(char[] base64Ciphertext)
        {
            if (base64Ciphertext == null) throw new ArgumentNullException(nameof(base64Ciphertext));
            if (base64Ciphertext.Length == 0) throw new ArgumentException(Properties.Localization.the_ciphertext_cannot_be_empty);

            return Decrypt(Convert.FromBase64CharArray(base64Ciphertext, 0, base64Ciphertext.Length));
        }

        public byte[] Decrypt(string base64Ciphertext)
        {
            if (base64Ciphertext == null) throw new ArgumentNullException(nameof(base64Ciphertext));
            if (base64Ciphertext.Length == 0) throw new ArgumentException(Properties.Localization.the_ciphertext_cannot_be_empty);
            
            return Decrypt(Convert.FromBase64String(base64Ciphertext));
        }

        public void Dispose()
        {
            this._cryptograph.Dispose();
            this._cryptograph = null;
        }
        #endregion ISymmetricCryptography interface implementation.
    }
}
