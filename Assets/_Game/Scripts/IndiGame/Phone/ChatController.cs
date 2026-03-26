using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using IndiGame.Core;

namespace IndiGame.Phone
{
    /// <summary>
    /// Gestiona el sistema de chat y mensajería de la aplicación QuickRun.
    /// </summary>
    public class ChatController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform contentTransform;
        [SerializeField] private GameObject leftBubblePrefab;
        [SerializeField] private GameObject rightBubblePrefab;

        [Header("Settings")]
        [SerializeField] private float defaultMessageDelay = 0.5f;

        private List<MessageData> messageHistory = new List<MessageData>();
        private Queue<MessageData> pendingMessages = new Queue<MessageData>();
        private bool isProcessingQueue = false;

        private void Start()
        {
            if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0f;
        }

        /// <summary>
        /// Inicia una secuencia de mensajes desde un ScriptableObject.
        /// </summary>
        /// <param name="sequence">La secuencia de mensajes a procesar.</param>
        public void PlaySequence(MessageSequence sequence)
        {
            if (sequence == null || sequence.messages == null) return;

            foreach (var msg in sequence.messages)
            {
                pendingMessages.Enqueue(msg);
            }

            if (!isProcessingQueue)
            {
                StartCoroutine(ProcessMessageQueue());
            }
        }

        /// <summary>
        /// Recibe un mensaje individual y lo procesa.
        /// </summary>
        /// <param name="data">Los datos del mensaje.</param>
        public void ReceiveMessage(MessageData data)
        {
            if (data == null) return;
            
            pendingMessages.Enqueue(data);
            
            if (!isProcessingQueue)
            {
                StartCoroutine(ProcessMessageQueue());
            }
        }

        private IEnumerator ProcessMessageQueue()
        {
            isProcessingQueue = true;

            while (pendingMessages.Count > 0)
            {
                MessageData currentMsg = pendingMessages.Dequeue();
                
                // Mostrar en UI
                InstantiateMessage(currentMsg);
                
                // Disparar evento global
                EventManager.Instance?.OnMessageReceived?.Invoke(currentMsg);

                // Notificación si no es del jugador
                if (!currentMsg.isPlayerResponse)
                {
                    AppNotification.ShowNewChatMessage(currentMsg.senderName, currentMsg.messageText);
                }

                // Esperar delay
                float delay = currentMsg.delayBeforeNext > 0 ? currentMsg.delayBeforeNext : defaultMessageDelay;
                yield return new WaitForSeconds(delay);
            }

            isProcessingQueue = false;
        }

        private void InstantiateMessage(MessageData data)
        {
            GameObject prefab = data.isPlayerResponse ? rightBubblePrefab : leftBubblePrefab;
            if (prefab == null) return;

            GameObject bubbleGo = Instantiate(prefab, contentTransform);
            
            // Configurar textos (Buscamos por nombre o componente)
            var texts = bubbleGo.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
            {
                if (t.gameObject.name.Contains("Name")) t.text = data.senderName;
                else if (t.gameObject.name.Contains("Text")) t.text = data.messageText;
                else if (t.gameObject.name.Contains("Time")) t.text = System.DateTime.Now.ToString("HH:mm");
            }

            messageHistory.Add(data);
            
            // Forzar actualización de layout y scroll
            Canvas.ForceUpdateCanvases();
            StartCoroutine(ScrollToBottom());
        }

        private IEnumerator ScrollToBottom()
        {
            yield return new WaitForEndOfFrame();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}