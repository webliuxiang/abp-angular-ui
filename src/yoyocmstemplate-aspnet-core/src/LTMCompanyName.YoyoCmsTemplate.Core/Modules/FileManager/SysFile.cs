using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.FileManager
{
    /// <summary>
    /// 文件
    /// </summary>
    [Table(AppConsts.TablePrefix + "SysFileInfos")]

    public class SysFile : AuditedEntity<Guid>
    {
        /// <summary>
        /// Maximum length of the <see cref="Code" /> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum depth of an UO hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// 父级 <see cref="SysFile" /> Id. 如果是根节点，值为null
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool Dir { get; set; }

        /// <summary>
        /// 是否为图片
        /// </summary>
        public bool IsImg { get; set; }

        /// <summary>
        /// 文件的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件原始名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 时间文件夹名称
        /// </summary>
        public string DateDirctoryName { get; set; }

        /// <summary>
        /// 对象类型（阿里云）
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// 存储类型（阿里云）
        /// </summary>
        public string StorageClass { get; set; }

        /// <summary>
        /// 修改时间戳（阿里云）
        /// </summary>
        public string TimeModified { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 格式化的文件大小
        /// </summary>
        public string FormattedSize { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }



        public bool IsHidden { get; set; }

        /// <summary>
        /// 记录文件的层次结构关系
        /// Example: "00001.00042.00005". 这是租户的唯一代码。 当然可以进行修改
        /// </summary>
        [Required]
        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 将子代码附加到父代码。 例如:如果parentCode = "00001"，则childCode = "00042"，然后返回"00001.00042"。
        /// </summary>
        /// <param name="parentCode"> 父类的代码。 如果父节点是根节点，则可以为空或空。 </param>
        /// <param name="childCode"> 子代码. </param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "子代码不能为空或者为null");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// 为给定的数字创建代码。
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers"> Numbers </param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// 计算给定代码的下一个代码。
        /// Example: if code = "00019.00055.00001" 返回 "00019.00055.00002".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets parent code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        /// <summary>
        /// Gets the last unit code.
        /// Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets relative code to the parent.
        /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code"> The code. </param>
        /// <param name="parentCode"> The parent code. </param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }
    }
}
