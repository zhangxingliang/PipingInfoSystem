using PipingInfoSystem.model;
using PipingInfoSystem.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipingInfoSystem
{
    /// <summary>
    /// 管道信息
    /// </summary>
    public class MPipingInfo
    {
        /// <summary>
        /// 管道检测信息
        /// </summary>
        public PipingDetectionInfo PipingDetectionInfo { get; set; }

        /// <summary>
        /// 管道图片信息列表
        /// </summary>
        public List<PipingPictureInfo> PipingPictureInfoList { get; set; }
    }
}
