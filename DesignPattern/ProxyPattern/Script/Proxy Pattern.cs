using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProxyPattern
{
    // �̹����� �ε��ϴ� �������̽�
    interface IImageLoader
    {
        Texture2D LoadImage(string path);
    }

    // �̹����� ������ �ε��ϴ� Ŭ����
    class ImageLoader : IImageLoader
    {
        public Texture2D LoadImage(string path)
        {
            // �̹��� �ε��ϴ� ���� ����
            Debug.Log("�̹����� �ε��մϴ�: " + path);
            return Resources.Load<Texture2D>(path);
        }
    }

    // �̹��� �ε��� ���� ������ �����ϴ� ���Ͻ� Ŭ����
    class ImageLoaderProxy : IImageLoader
    {
        private ImageLoader imageLoader;
        private Dictionary<string, Texture2D> imageCache;

        public ImageLoaderProxy()
        {
            imageLoader = new ImageLoader();
            imageCache = new Dictionary<string, Texture2D>();
        }

        public Texture2D LoadImage(string path)
        {
            // �̹����� ĳ�ÿ� �ִ��� Ȯ��
            if (imageCache.ContainsKey(path))
            {
                Debug.Log("ĳ�õ� �̹����� ��ȯ�մϴ�: " + path);
                return imageCache[path];
            }
            else
            {
                // ĳ�ÿ� ���� ��� ���� �δ��� ���� �̹����� �ε��ϰ� ĳ�ÿ� �߰�
                Texture2D image = imageLoader.LoadImage(path);
                imageCache.Add(path, image);
                return image;
            }
        }
    }

    public class Client : MonoBehaviour
    {
        void Start()
        {
            // �̹��� �δ� ���Ͻø� ���� �̹����� �ε�
            IImageLoader imageLoader = new ImageLoaderProxy();
            Texture2D image1 = imageLoader.LoadImage("Images/Image1");
            Texture2D image2 = imageLoader.LoadImage("Images/Image2");
            Texture2D image3 = imageLoader.LoadImage("Images/Image1"); // �̹���1�� ĳ�ÿ� �����Ƿ� �ٽ� �ε����� ����
        }
    }
}
