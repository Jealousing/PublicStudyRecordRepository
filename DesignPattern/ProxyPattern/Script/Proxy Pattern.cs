using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProxyPattern
{
    // 이미지를 로드하는 인터페이스
    interface IImageLoader
    {
        Texture2D LoadImage(string path);
    }

    // 이미지를 실제로 로드하는 클래스
    class ImageLoader : IImageLoader
    {
        public Texture2D LoadImage(string path)
        {
            // 이미지 로드하는 실제 구현
            Debug.Log("이미지를 로드합니다: " + path);
            return Resources.Load<Texture2D>(path);
        }
    }

    // 이미지 로딩에 대한 접근을 제어하는 프록시 클래스
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
            // 이미지가 캐시에 있는지 확인
            if (imageCache.ContainsKey(path))
            {
                Debug.Log("캐시된 이미지를 반환합니다: " + path);
                return imageCache[path];
            }
            else
            {
                // 캐시에 없는 경우 실제 로더를 통해 이미지를 로드하고 캐시에 추가
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
            // 이미지 로더 프록시를 통해 이미지를 로드
            IImageLoader imageLoader = new ImageLoaderProxy();
            Texture2D image1 = imageLoader.LoadImage("Images/Image1");
            Texture2D image2 = imageLoader.LoadImage("Images/Image2");
            Texture2D image3 = imageLoader.LoadImage("Images/Image1"); // 이미지1은 캐시에 있으므로 다시 로드하지 않음
        }
    }
}
