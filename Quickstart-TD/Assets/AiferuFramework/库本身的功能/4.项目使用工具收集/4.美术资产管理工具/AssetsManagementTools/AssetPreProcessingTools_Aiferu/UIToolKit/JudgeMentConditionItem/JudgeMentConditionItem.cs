# if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;

namespace AiferuFramework.AssetsManagementTools
{
    public class JudgeMentConditionItem : VisualElement
    {
        VisualElement root;

        private string AssetPreprocessingToolsFilePath = "";
        private JudgeMentConditionItemConfig JudgeMentConditionItemConfig;
        //主窗口的引用
        private AssetPreprocessingTools preprocessingTools;
        /// <summary>
        /// 该模块的UXML文件相对于父插件目录的路径
        /// </summary>
        private static string UXMLFilePath = "/UIToolKit/JudgeMentConditionItem/JudgeMentConditionItem.uxml";

        #region UIToolKit属性
        private TextField m_textField_RuleName;
        private TextField m_textField_Keyword;
        private EnumField m_enumField_JudgmentType;
        private SliderInt m_sliderInt_Priorty;
        private Label m_label_Priorty;
        private Label m_lable_RuleName;
        private Button m_button_Delete;
        #endregion


        /// <summary>
        /// 创建一个新的JudegMentConditionItem
        /// </summary>
        /// <param name="AssetPreprocessingToolsFilePath">输入插件的相对路径</param>
        public JudgeMentConditionItem(string AssetPreprocessingToolsFilePath, JudgeMentConditionItemConfig JudgeMentConditionItemConfig, AssetPreprocessingTools assetPreprocessingTools)
        {
            //初始化
            this.AssetPreprocessingToolsFilePath = AssetPreprocessingToolsFilePath;
            this.JudgeMentConditionItemConfig = JudgeMentConditionItemConfig;
            this.preprocessingTools = assetPreprocessingTools;
            // Each editor window contains a root VisualElement object
            root = this;
            // Import UXML 
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetPreprocessingToolsFilePath + UXMLFilePath);
            //克隆UXML文件中的东西并初始化
            visualTree.CloneTree(root);


            UIToolKitPropertyInitialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="AssetPreprocessingToolsFilePath"></param>
        /// <param name="JudgeMentConditionItemConfigs"></param>
        private void Initialize(string AssetPreprocessingToolsFilePath, JudgeMentConditionItemConfig JudgeMentConditionItemConfig)
        {
            this.AssetPreprocessingToolsFilePath = AssetPreprocessingToolsFilePath;
            this.JudgeMentConditionItemConfig = JudgeMentConditionItemConfig;
            // Each editor window contains a root VisualElement object
            root = this;
            // Import UXML 
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetPreprocessingToolsFilePath + UXMLFilePath);
            //克隆UXML文件中的东西并初始化
            visualTree.CloneTree(root);
        }

        /// <summary>
        /// 初始化UIToolKit属性
        /// </summary>
        private void UIToolKitPropertyInitialize()
        {
            SerializedObject so = new SerializedObject(JudgeMentConditionItemConfig);

            m_textField_RuleName = root.Q<TextField>("TextField_RuleName");
            //数据绑定
            m_textField_RuleName.Bind(so);
            m_textField_RuleName.bindingPath = "RuleName";

            m_textField_Keyword = root.Q<TextField>("TextField_Keyword");
            //数据绑定
            m_textField_Keyword.Bind(so);
            m_textField_Keyword.bindingPath = "keyword";

            m_enumField_JudgmentType = root.Q<EnumField>("EnumField_JudgmentType");
            //数据绑定
            m_enumField_JudgmentType.Init(JudgeMentConditionItemConfig.judgmentType);
            m_enumField_JudgmentType.Bind(so);
            m_enumField_JudgmentType.bindingPath = "judgmentType";

            m_sliderInt_Priorty = root.Q<SliderInt>("SliderInt_Priorty");
            //数据绑定
            m_sliderInt_Priorty.Bind(so);
            m_sliderInt_Priorty.bindingPath = "priority";

            m_label_Priorty = root.Q<Label>("Label_Priorty");
            //数据绑定
            m_label_Priorty.Bind(so);
            m_label_Priorty.bindingPath = "priority";

            m_lable_RuleName = root.Q<Label>("Label_RuleName");
            //数据绑定
            m_lable_RuleName.Bind(so);
            m_lable_RuleName.bindingPath = "RuleName";

            m_button_Delete = root.Q<Button>("Button_Delete");
            m_button_Delete.clicked += m_button_Delete_onclicked;
        }

        /// <summary>
        /// 删除按钮点击
        /// </summary>
        private void m_button_Delete_onclicked()
        {
            string assetsPath = AssetPreprocessingToolsFilePath + "/JudgeMentConditionItemConfigData";

            //获取当前配置文件的路径
            Debug.Log(JudgeMentConditionItemConfig.GetHashCode());
            Debug.Log(JudgeMentConditionItemConfig.GetInstanceID());
            //删除文件和对应的引用
            Debug.Log(JudgeMentConditionItemConfig.filePath);

            //删除引用
            switch (((BasePreprocessorConfig)(Selection.activeObject)).FileType)
            {
                case BasePreprocessorConfig.InputFileType.Texture:
                    ((TexturePreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Remove(JudgeMentConditionItemConfig);

                    break;
                case BasePreprocessorConfig.InputFileType.Model:
                    ((ModelPreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Remove(JudgeMentConditionItemConfig);
                    break;
                case BasePreprocessorConfig.InputFileType.Audio:
                    ((AudioPreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Remove(JudgeMentConditionItemConfig);
                    break;
                default:
                    Debug.Log("BasePreprocessorConfig.cs的代码可能有问题");
                    break;
            }
            //删除文件
            AssetDatabase.DeleteAsset(JudgeMentConditionItemConfig.filePath);

            //保存序列化对象
            EditorUtility.SetDirty(Selection.activeObject);
            preprocessingTools.Refresh();
            Debug.Log("创建判断条件成功");
            //刷新

        }
    }
}
#endif